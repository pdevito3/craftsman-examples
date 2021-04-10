namespace Billing.WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Billing.Core.Dtos.InsuranceProvider;
    using Billing.Core.Wrappers;
    using System.Threading;
    using MediatR;
    using static Billing.WebApi.Features.InsuranceProviders.GetInsuranceProviderList;
    using static Billing.WebApi.Features.InsuranceProviders.GetInsuranceProvider;
    using static Billing.WebApi.Features.InsuranceProviders.AddInsuranceProvider;
    using static Billing.WebApi.Features.InsuranceProviders.DeleteInsuranceProvider;
    using static Billing.WebApi.Features.InsuranceProviders.UpdateInsuranceProvider;
    using static Billing.WebApi.Features.InsuranceProviders.PatchInsuranceProvider;

    [ApiController]
    [Route("api/InsuranceProviders")]
    [ApiVersion("1.0")]
    public class InsuranceProvidersController: Controller
    {
        private readonly IMediator _mediator;

        public InsuranceProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets a list of all InsuranceProviders.
        /// </summary>
        /// <response code="200">InsuranceProvider list returned successfully.</response>
        /// <response code="400">InsuranceProvider has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the InsuranceProvider.</response>
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
        [ProducesResponseType(typeof(Response<IEnumerable<InsuranceProviderDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetInsuranceProviders")]
        public async Task<IActionResult> GetInsuranceProviders([FromQuery] InsuranceProviderParametersDto insuranceProviderParametersDto)
        {
            // add error handling
            var query = new InsuranceProviderListQuery(insuranceProviderParametersDto);
            var queryResponse = await _mediator.Send(query);

            var paginationMetadata = new
            {
                totalCount = queryResponse.TotalCount,
                pageSize = queryResponse.PageSize,
                currentPageSize = queryResponse.CurrentPageSize,
                currentStartIndex = queryResponse.CurrentStartIndex,
                currentEndIndex = queryResponse.CurrentEndIndex,
                pageNumber = queryResponse.PageNumber,
                totalPages = queryResponse.TotalPages,
                hasPrevious = queryResponse.HasPrevious,
                hasNext = queryResponse.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var response = new Response<IEnumerable<InsuranceProviderDto>>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single InsuranceProvider by ID.
        /// </summary>
        /// <response code="200">InsuranceProvider record returned successfully.</response>
        /// <response code="400">InsuranceProvider has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the InsuranceProvider.</response>
        [ProducesResponseType(typeof(Response<InsuranceProviderDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{insuranceProviderId}", Name = "GetInsuranceProvider")]
        public async Task<ActionResult<InsuranceProviderDto>> GetInsuranceProvider(Guid insuranceProviderId)
        {
            // add error handling
            var query = new InsuranceProviderQuery(insuranceProviderId);
            var queryResponse = await _mediator.Send(query);

            var response = new Response<InsuranceProviderDto>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new InsuranceProvider record.
        /// </summary>
        /// <response code="201">InsuranceProvider created.</response>
        /// <response code="400">InsuranceProvider has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the InsuranceProvider.</response>
        [ProducesResponseType(typeof(Response<InsuranceProviderDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<InsuranceProviderDto>> AddInsuranceProvider([FromBody]InsuranceProviderForCreationDto insuranceProviderForCreation)
        {
            // add error handling
            var command = new AddInsuranceProviderCommand(insuranceProviderForCreation);
            var commandResponse = await _mediator.Send(command);
            var response = new Response<InsuranceProviderDto>(commandResponse);

            return CreatedAtRoute("GetInsuranceProvider",
                new { commandResponse.InsuranceProviderId },
                response);
        }
        
        /// <summary>
        /// Deletes an existing InsuranceProvider record.
        /// </summary>
        /// <response code="201">InsuranceProvider deleted.</response>
        /// <response code="400">InsuranceProvider has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the InsuranceProvider.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{insuranceProviderId}")]
        public async Task<ActionResult> DeleteInsuranceProvider(Guid insuranceProviderId)
        {
            // add error handling
            var command = new DeleteInsuranceProviderCommand(insuranceProviderId);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing InsuranceProvider.
        /// </summary>
        /// <response code="201">InsuranceProvider updated.</response>
        /// <response code="400">InsuranceProvider has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the InsuranceProvider.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{insuranceProviderId}")]
        public async Task<IActionResult> UpdateInsuranceProvider(Guid insuranceProviderId, InsuranceProviderForUpdateDto insuranceProvider)
        {
            // add error handling
            var command = new UpdateInsuranceProviderCommand(insuranceProviderId, insuranceProvider);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing InsuranceProvider.
        /// </summary>
        /// <response code="201">InsuranceProvider updated.</response>
        /// <response code="400">InsuranceProvider has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the InsuranceProvider.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{insuranceProviderId}")]
        public async Task<IActionResult> PartiallyUpdateInsuranceProvider(Guid insuranceProviderId, JsonPatchDocument<InsuranceProviderForUpdateDto> patchDoc)
        {
            // add error handling
            var command = new PatchInsuranceProviderCommand(insuranceProviderId, patchDoc);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}