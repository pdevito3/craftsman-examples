namespace Ordering.WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Ordering.Core.Dtos.Patient;
    using Ordering.Core.Wrappers;
    using System.Threading;
    using MediatR;
    using Ordering.WebApi.Features.Patients;

    [ApiController]
    [Route("api/Patients")]
    [ApiVersion("1.0")]
    public class PatientsController: ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets a list of all Patients.
        /// </summary>
        /// <response code="200">Patient list returned successfully.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
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
        [ProducesResponseType(typeof(Response<IEnumerable<PatientDto>>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetPatients")]
        public async Task<IActionResult> GetPatients([FromQuery] PatientParametersDto patientParametersDto)
        {
            // add error handling
            var query = new GetPatientList.PatientListQuery(patientParametersDto);
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

            var response = new Response<IEnumerable<PatientDto>>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single Patient by ID.
        /// </summary>
        /// <response code="200">Patient record returned successfully.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(typeof(Response<PatientDto>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{patientId}", Name = "GetPatient")]
        public async Task<ActionResult<PatientDto>> GetPatient(Guid patientId)
        {
            // add error handling
            var query = new GetPatient.PatientQuery(patientId);
            var queryResponse = await _mediator.Send(query);

            var response = new Response<PatientDto>(queryResponse);
            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new Patient record.
        /// </summary>
        /// <response code="201">Patient created.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="409">A record already exists with this primary key.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(typeof(Response<PatientDto>), 201)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(typeof(Response<>), 409)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<PatientDto>> AddPatient([FromBody]PatientForCreationDto patientForCreation)
        {
            // add error handling
            var command = new AddPatient.AddPatientCommand(patientForCreation);
            var commandResponse = await _mediator.Send(command);
            var response = new Response<PatientDto>(commandResponse);

            return CreatedAtRoute("GetPatient",
                new { commandResponse.PatientId },
                response);
        }
        
        /// <summary>
        /// Deletes an existing Patient record.
        /// </summary>
        /// <response code="204">Patient deleted.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{patientId}")]
        public async Task<ActionResult> DeletePatient(Guid patientId)
        {
            // add error handling
            var command = new DeletePatient.DeletePatientCommand(patientId);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing Patient.
        /// </summary>
        /// <response code="204">Patient updated.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{patientId}")]
        public async Task<IActionResult> UpdatePatient(Guid patientId, PatientForUpdateDto patient)
        {
            // add error handling
            var command = new UpdatePatient.UpdatePatientCommand(patientId, patient);
            await _mediator.Send(command);

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing Patient.
        /// </summary>
        /// <response code="204">Patient updated.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{patientId}")]
        public async Task<IActionResult> PartiallyUpdatePatient(Guid patientId, JsonPatchDocument<PatientForUpdateDto> patchDoc)
        {
            // add error handling
            var command = new PatchPatient.PatchPatientCommand(patientId, patchDoc);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}