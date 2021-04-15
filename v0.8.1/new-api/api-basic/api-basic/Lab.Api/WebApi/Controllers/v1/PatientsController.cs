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