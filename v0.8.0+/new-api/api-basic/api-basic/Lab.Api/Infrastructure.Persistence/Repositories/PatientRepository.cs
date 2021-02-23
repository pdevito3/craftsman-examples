namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.Patient;
    using Application.Interfaces.Patient;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class PatientRepository : IPatientRepository
    {
        private LabDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public PatientRepository(LabDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Patient>> GetPatientsAsync(PatientParametersDto patientParameters)
        {
            if (patientParameters == null)
            {
                throw new ArgumentNullException(nameof(patientParameters));
            }

            var collection = _context.Patients
                as IQueryable<Patient>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = patientParameters.SortOrder ?? "PatientId",
                Filters = patientParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Patient>.CreateAsync(collection,
                patientParameters.PageNumber,
                patientParameters.PageSize);
        }

        public async Task<Patient> GetPatientAsync(int patientId)
        {
            // include marker -- requires return _context.Patients as it's own line with no extra text -- do not delete this comment
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        public Patient GetPatient(int patientId)
        {
            // include marker -- requires return _context.Patients as it's own line with no extra text -- do not delete this comment
            return _context.Patients
                .FirstOrDefault(p => p.PatientId == patientId);
        }

        public async Task AddPatient(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(Patient));
            }

            await _context.Patients.AddAsync(patient);
        }

        public void DeletePatient(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(Patient));
            }

            _context.Patients.Remove(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}