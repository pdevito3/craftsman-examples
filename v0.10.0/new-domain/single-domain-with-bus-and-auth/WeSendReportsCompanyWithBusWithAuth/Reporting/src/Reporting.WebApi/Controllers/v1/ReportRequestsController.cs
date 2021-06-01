namespace Reporting.WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.Core.Wrappers;
    using System.Threading;
    using MediatR;
    using Reporting.WebApi.Features.ReportRequests;

    [ApiController]
    [Route("api/ReportRequests")]
    [ApiVersion("1.0")]
    public class ReportRequestsController: ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets a list of all ReportRequests.
        /// </summary>
        /// <response code="200">ReportRequest list returned successfully.</response>
        /// <response code="400">ReportRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the ReportRequest.</response>
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
        [ProducesResponseType(typeof(Response<IEnumerable<ReportRequestDto>>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetReportRequests")]
        public async Task<IActionResult> GetReportRequests([FromQuery] ReportRequestParametersDto reportRequestParametersDto)
        {
            // add error handling
            var query = new GetReportRequestList.ReportRequestListQuery(reportRequestParametersDto);
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

            var response = new Response<IEnumerable<ReportRequestDto>>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single ReportRequest by ID.
        /// </summary>
        /// <response code="200">ReportRequest record returned successfully.</response>
        /// <response code="400">ReportRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the ReportRequest.</response>
        [ProducesResponseType(typeof(Response<ReportRequestDto>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{reportId}", Name = "GetReportRequest")]
        public async Task<ActionResult<ReportRequestDto>> GetReportRequest(Guid reportId)
        {
            // add error handling
            var query = new GetReportRequest.ReportRequestQuery(reportId);
            var queryResponse = await _mediator.Send(query);

            var response = new Response<ReportRequestDto>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new ReportRequest record.
        /// </summary>
        /// <response code="201">ReportRequest created.</response>
        /// <response code="400">ReportRequest has missing/invalid values.</response>
        /// <response code="409">A record already exists with this primary key.</response>
        /// <response code="500">There was an error on the server while creating the ReportRequest.</response>
        [ProducesResponseType(typeof(Response<ReportRequestDto>), 201)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(typeof(Response<>), 409)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<ReportRequestDto>> AddReportRequest([FromBody]ReportRequestForCreationDto reportRequestForCreation)
        {
            // add error handling
            var command = new AddReportRequest.AddReportRequestCommand(reportRequestForCreation);
            var commandResponse = await _mediator.Send(command);
            var response = new Response<ReportRequestDto>(commandResponse);

            return CreatedAtRoute("GetReportRequest",
                new { commandResponse.ReportId },
                response);
        }
        
        /// <summary>
        /// Deletes an existing ReportRequest record.
        /// </summary>
        /// <response code="204">ReportRequest deleted.</response>
        /// <response code="400">ReportRequest has missing/invalid values.</response>
        /// <response code="401">This request was not able to be authenticated.</response>
        /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
        /// <response code="500">There was an error on the server while creating the ReportRequest.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(typeof(Response<>), 401)]
        [ProducesResponseType(typeof(Response<>), 403)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "CanDeleteReportRequest")]
        [Produces("application/json")]
        [HttpDelete("{reportId}")]
        public async Task<ActionResult> DeleteReportRequest(Guid reportId)
        {
            // add error handling
            var command = new DeleteReportRequest.DeleteReportRequestCommand(reportId);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing ReportRequest.
        /// </summary>
        /// <response code="204">ReportRequest updated.</response>
        /// <response code="400">ReportRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the ReportRequest.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{reportId}")]
        public async Task<IActionResult> UpdateReportRequest(Guid reportId, ReportRequestForUpdateDto reportRequest)
        {
            // add error handling
            var command = new UpdateReportRequest.UpdateReportRequestCommand(reportId, reportRequest);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing ReportRequest.
        /// </summary>
        /// <response code="204">ReportRequest updated.</response>
        /// <response code="400">ReportRequest has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the ReportRequest.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{reportId}")]
        public async Task<IActionResult> PartiallyUpdateReportRequest(Guid reportId, JsonPatchDocument<ReportRequestForUpdateDto> patchDoc)
        {
            // add error handling
            var command = new PatchReportRequest.PatchReportRequestCommand(reportId, patchDoc);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}