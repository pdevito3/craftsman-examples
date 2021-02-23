namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.Patient;
    using Application.Interfaces.Patient;
    using Application.Validation.Patient;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/Patients")]
    [ApiVersion("1.0")]
    public class PatientsController: Controller
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientsController(IPatientRepository patientRepository
            , IMapper mapper)
        {
            _patientRepository = patientRepository ??
                throw new ArgumentNullException(nameof(patientRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
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
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetPatients")]
        public async Task<IActionResult> GetPatients([FromQuery] PatientParametersDto patientParametersDto)
        {
            var patientsFromRepo = await _patientRepository.GetPatientsAsync(patientParametersDto);

            var paginationMetadata = new
            {
                totalCount = patientsFromRepo.TotalCount,
                pageSize = patientsFromRepo.PageSize,
                currentPageSize = patientsFromRepo.CurrentPageSize,
                currentStartIndex = patientsFromRepo.CurrentStartIndex,
                currentEndIndex = patientsFromRepo.CurrentEndIndex,
                pageNumber = patientsFromRepo.PageNumber,
                totalPages = patientsFromRepo.TotalPages,
                hasPrevious = patientsFromRepo.HasPrevious,
                hasNext = patientsFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var patientsDto = _mapper.Map<IEnumerable<PatientDto>>(patientsFromRepo);
            var response = new Response<IEnumerable<PatientDto>>(patientsDto);

            return Ok(response);
        }
        
        /// <summary>
        /// Gets a single Patient by ID.
        /// </summary>
        /// <response code="200">Patient record returned successfully.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(typeof(Response<PatientDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{patientId}", Name = "GetPatient")]
        public async Task<ActionResult<PatientDto>> GetPatient(int patientId)
        {
            var patientFromRepo = await _patientRepository.GetPatientAsync(patientId);

            if (patientFromRepo == null)
            {
                return NotFound();
            }

            var patientDto = _mapper.Map<PatientDto>(patientFromRepo);
            var response = new Response<PatientDto>(patientDto);

            return Ok(response);
        }
        
        /// <summary>
        /// Creates a new Patient record.
        /// </summary>
        /// <response code="201">Patient created.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(typeof(Response<PatientDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<PatientDto>> AddPatient([FromBody]PatientForCreationDto patientForCreation)
        {
            var validationResults = new PatientForCreationDtoValidator().Validate(patientForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            var patient = _mapper.Map<Patient>(patientForCreation);
            await _patientRepository.AddPatient(patient);
            var saveSuccessful = await _patientRepository.SaveAsync();

            if(saveSuccessful)
            {
                var patientFromRepo = await _patientRepository.GetPatientAsync(patient.PatientId);
                var patientDto = _mapper.Map<PatientDto>(patientFromRepo);
                var response = new Response<PatientDto>(patientDto);
                
                return CreatedAtRoute("GetPatient",
                    new { patientDto.PatientId },
                    response);
            }

            return StatusCode(500);
        }
        
        /// <summary>
        /// Deletes an existing Patient record.
        /// </summary>
        /// <response code="201">Patient deleted.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{patientId}")]
        public async Task<ActionResult> DeletePatient(int patientId)
        {
            var patientFromRepo = await _patientRepository.GetPatientAsync(patientId);

            if (patientFromRepo == null)
            {
                return NotFound();
            }

            _patientRepository.DeletePatient(patientFromRepo);
            await _patientRepository.SaveAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Updates an entire existing Patient.
        /// </summary>
        /// <response code="201">Patient updated.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{patientId}")]
        public async Task<IActionResult> UpdatePatient(int patientId, PatientForUpdateDto patient)
        {
            var patientFromRepo = await _patientRepository.GetPatientAsync(patientId);

            if (patientFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new PatientForUpdateDtoValidator().Validate(patient);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                //return ValidationProblem();
            }

            _mapper.Map(patient, patientFromRepo);
            _patientRepository.UpdatePatient(patientFromRepo);

            await _patientRepository.SaveAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Updates specific properties on an existing Patient.
        /// </summary>
        /// <response code="201">Patient updated.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{patientId}")]
        public async Task<IActionResult> PartiallyUpdatePatient(int patientId, JsonPatchDocument<PatientForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingPatient = await _patientRepository.GetPatientAsync(patientId);

            if (existingPatient == null)
            {
                return NotFound();
            }

            var patientToPatch = _mapper.Map<PatientForUpdateDto>(existingPatient); // map the patient we got from the database to an updatable patient model
            patchDoc.ApplyTo(patientToPatch, ModelState); // apply patchdoc updates to the updatable patient

            if (!TryValidateModel(patientToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(patientToPatch, existingPatient); // apply updates from the updatable patient to the db entity so we can apply the updates to the database
            _patientRepository.UpdatePatient(existingPatient); // apply business updates to data if needed

            await _patientRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}