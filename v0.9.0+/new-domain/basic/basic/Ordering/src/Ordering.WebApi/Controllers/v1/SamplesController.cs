namespace Ordering.WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Ordering.Core.Dtos.Sample;
    using Ordering.Core.Wrappers;
    using System.Threading;
    using MediatR;
    using static Ordering.WebApi.Features.Samples.GetSampleList;
    using static Ordering.WebApi.Features.Samples.GetSample;
    using static Ordering.WebApi.Features.Samples.AddSample;
    using static Ordering.WebApi.Features.Samples.DeleteSample;
    using static Ordering.WebApi.Features.Samples.UpdateSample;
    using static Ordering.WebApi.Features.Samples.PatchSample;

    [ApiController]
    [Route("api/Samples")]
    [ApiVersion("1.0")]
    public class SamplesController: Controller
    {
        private readonly IMediator _mediator;

        public SamplesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets a list of all Samples.
        /// </summary>
        /// <response code="200">Sample list returned successfully.</response>
        /// <response code="400">Sample has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Sample.</response>
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
        [ProducesResponseType(typeof(Response<IEnumerable<SampleDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetSamples")]
        public async Task<IActionResult> GetSamples([FromQuery] SampleParametersDto sampleParametersDto)
        {
            // add error handling
            var query = new SampleListQuery(sampleParametersDto);
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

            var response = new Response<IEnumerable<SampleDto>>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single Sample by ID.
        /// </summary>
        /// <response code="200">Sample record returned successfully.</response>
        /// <response code="400">Sample has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Sample.</response>
        [ProducesResponseType(typeof(Response<SampleDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{sampleId}", Name = "GetSample")]
        public async Task<ActionResult<SampleDto>> GetSample(Guid sampleId)
        {
            // add error handling
            var query = new SampleQuery(sampleId);
            var queryResponse = await _mediator.Send(query);

            var response = new Response<SampleDto>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new Sample record.
        /// </summary>
        /// <response code="201">Sample created.</response>
        /// <response code="400">Sample has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Sample.</response>
        [ProducesResponseType(typeof(Response<SampleDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<SampleDto>> AddSample([FromBody]SampleForCreationDto sampleForCreation)
        {
            // add error handling
            var command = new AddSampleCommand(sampleForCreation);
            var commandResponse = await _mediator.Send(command);
            var response = new Response<SampleDto>(commandResponse);

            return CreatedAtRoute("GetSample",
                new { commandResponse.SampleId },
                response);
        }
        
        /// <summary>
        /// Deletes an existing Sample record.
        /// </summary>
        /// <response code="204">Sample deleted.</response>
        /// <response code="400">Sample has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Sample.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{sampleId}")]
        public async Task<ActionResult> DeleteSample(Guid sampleId)
        {
            // add error handling
            var command = new DeleteSampleCommand(sampleId);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing Sample.
        /// </summary>
        /// <response code="204">Sample updated.</response>
        /// <response code="400">Sample has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Sample.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{sampleId}")]
        public async Task<IActionResult> UpdateSample(Guid sampleId, SampleForUpdateDto sample)
        {
            // add error handling
            var command = new UpdateSampleCommand(sampleId, sample);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing Sample.
        /// </summary>
        /// <response code="204">Sample updated.</response>
        /// <response code="400">Sample has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Sample.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{sampleId}")]
        public async Task<IActionResult> PartiallyUpdateSample(Guid sampleId, JsonPatchDocument<SampleForUpdateDto> patchDoc)
        {
            // add error handling
            var command = new PatchSampleCommand(sampleId, patchDoc);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}