namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.Recipe;
    using Application.Interfaces.Recipe;
    using Application.Validation.Recipe;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/Recipes")]
    [ApiVersion("1.0")]
    public class RecipesController: Controller
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RecipesController(IRecipeRepository recipeRepository
            , IMapper mapper)
        {
            _recipeRepository = recipeRepository ??
                throw new ArgumentNullException(nameof(recipeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
        /// <summary>
        /// Gets a list of all Recipes.
        /// </summary>
        /// <response code="200">Recipe list returned successfully.</response>
        /// <response code="400">Recipe has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the Recipe.</response>
        /// <remarks>
        /// Requests can be narrowed down with a variety of query string values:
        /// ## Query String Parameters
        /// - **PageNumber**: An integer value that designates the page of records that should be returned.
        /// - **PageSize**: An integer value that designates the number of records returned on the given page that you would like to return. This value is capped by the internal MaxPageSize.
        /// - **SortOrder**: A comma delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
        /// - **Filters**: A comma delimited list of fields to filter by formatted as `{Name}{Operator}{Value}` where
        ///     - {Name} is the name of a filterable property. You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if LikeCount or CommentCount is >10
        ///     - {Operator} is one of the Operators below
        ///     - {Value} is the value to use for filtering. You can also have multiple values (for OR logic) by using a pipe delimiter, eg.`Title@= new|hot` will return posts with titles that contain the text "new" or "hot"
        ///
        ///    | Operator | Meaning                       | Operator  | Meaning                                      |
        ///    | -------- | ----------------------------- | --------- | -------------------------------------------- |
        ///    | `==`     | Equals                        |  `!@=`    | Does not Contains                            |
        ///    | `!=`     | Not equals                    |  `!_=`    | Does not Starts with                         |
        ///    | `>`      | Greater than                  |  `@=*`    | Case-insensitive string Contains             |
        ///    | `&lt;`   | Less than                     |  `_=*`    | Case-insensitive string Starts with          |
        ///    | `>=`     | Greater than or equal to      |  `==*`    | Case-insensitive string Equals               |
        ///    | `&lt;=`  | Less than or equal to         |  `!=*`    | Case-insensitive string Not equals           |
        ///    | `@=`     | Contains                      |  `!@=*`   | Case-insensitive string does not Contains    |
        ///    | `_=`     | Starts with                   |  `!_=*`   | Case-insensitive string does not Starts with |
        /// </remarks>
        [ProducesResponseType(typeof(Response<IEnumerable<RecipeDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanReadRecipes")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetRecipes")]
        public async Task<IActionResult> GetRecipes([FromQuery] RecipeParametersDto recipeParametersDto)
        {
            var recipesFromRepo = await _recipeRepository.GetRecipesAsync(recipeParametersDto);

            var paginationMetadata = new
            {
                totalCount = recipesFromRepo.TotalCount,
                pageSize = recipesFromRepo.PageSize,
                currentPageSize = recipesFromRepo.CurrentPageSize,
                currentStartIndex = recipesFromRepo.CurrentStartIndex,
                currentEndIndex = recipesFromRepo.CurrentEndIndex,
                pageNumber = recipesFromRepo.PageNumber,
                totalPages = recipesFromRepo.TotalPages,
                hasPrevious = recipesFromRepo.HasPrevious,
                hasNext = recipesFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(recipesFromRepo);
            var response = new Response<IEnumerable<RecipeDto>>(recipesDto);

            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single Recipe by ID.
        /// </summary>
        /// <response code="200">Recipe record returned successfully.</response>
        /// <response code="400">Recipe has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the Recipe.</response>
        [ProducesResponseType(typeof(Response<RecipeDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanReadRecipes")]
        [Produces("application/json")]
        [HttpGet("{recipeId}", Name = "GetRecipe")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int recipeId)
        {
            var recipeFromRepo = await _recipeRepository.GetRecipeAsync(recipeId);

            if (recipeFromRepo == null)
            {
                return NotFound();
            }

            var recipeDto = _mapper.Map<RecipeDto>(recipeFromRepo);
            var response = new Response<RecipeDto>(recipeDto);

            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new Recipe record.
        /// </summary>
        /// <response code="201">Recipe created.</response>
        /// <response code="400">Recipe has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the Recipe.</response>
        [ProducesResponseType(typeof(Response<RecipeDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanAddRecipes")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<RecipeDto>> AddRecipe([FromBody]RecipeForCreationDto recipeForCreation)
        {
            var validationResults = new RecipeForCreationDtoValidator().Validate(recipeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var recipe = _mapper.Map<Recipe>(recipeForCreation);
            await _recipeRepository.AddRecipe(recipe);
            var saveSuccessful = await _recipeRepository.SaveAsync();

            if(saveSuccessful)
            {
                var recipeFromRepo = await _recipeRepository.GetRecipeAsync(recipe.RecipeId);
                var recipeDto = _mapper.Map<RecipeDto>(recipeFromRepo);
                var response = new Response<RecipeDto>(recipeDto);
                
                return CreatedAtRoute("GetRecipe",
                    new { recipeDto.RecipeId },
                    response);
            }

            return StatusCode(500);
        }
        
        /// <summary>
        /// Deletes an existing Recipe record.
        /// </summary>
        /// <response code="201">Recipe deleted.</response>
        /// <response code="400">Recipe has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the Recipe.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanDeleteRecipes")]
        [Produces("application/json")]
        [HttpDelete("{recipeId}")]
        public async Task<ActionResult> DeleteRecipe(int recipeId)
        {
            var recipeFromRepo = await _recipeRepository.GetRecipeAsync(recipeId);

            if (recipeFromRepo == null)
            {
                return NotFound();
            }

            _recipeRepository.DeleteRecipe(recipeFromRepo);
            await _recipeRepository.SaveAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing Recipe.
        /// </summary>
        /// <response code="201">Recipe updated.</response>
        /// <response code="400">Recipe has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the Recipe.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanUpdateRecipes")]
        [Produces("application/json")]
        [HttpPut("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, RecipeForUpdateDto recipe)
        {
            var recipeFromRepo = await _recipeRepository.GetRecipeAsync(recipeId);

            if (recipeFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new RecipeForUpdateDtoValidator().Validate(recipe);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(recipe, recipeFromRepo);
            _recipeRepository.UpdateRecipe(recipeFromRepo);

            await _recipeRepository.SaveAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing Recipe.
        /// </summary>
        /// <response code="201">Recipe updated.</response>
        /// <response code="400">Recipe has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the Recipe.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanUpdateRecipes")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{recipeId}")]
        public async Task<IActionResult> PartiallyUpdateRecipe(int recipeId, JsonPatchDocument<RecipeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingRecipe = await _recipeRepository.GetRecipeAsync(recipeId);

            if (existingRecipe == null)
            {
                return NotFound();
            }

            var recipeToPatch = _mapper.Map<RecipeForUpdateDto>(existingRecipe); // map the recipe we got from the database to an updatable recipe model
            patchDoc.ApplyTo(recipeToPatch, ModelState); // apply patchdoc updates to the updatable recipe

            if (!TryValidateModel(recipeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(recipeToPatch, existingRecipe); // apply updates from the updatable recipe to the db entity so we can apply the updates to the database
            _recipeRepository.UpdateRecipe(existingRecipe); // apply business updates to data if needed

            await _recipeRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}