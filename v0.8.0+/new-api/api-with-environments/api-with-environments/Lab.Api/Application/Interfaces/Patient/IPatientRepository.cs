namespace Application.Interfaces.Patient
{
    using System;
    using Application.Dtos.Patient;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IPatientRepository
    {
        Task<PagedList<Patient>> GetPatientsAsync(PatientParametersDto PatientParameters);
        Task<Patient> GetPatientAsync(int PatientId);
        Patient GetPatient(int PatientId);
        Task AddPatient(Patient patient);
        void DeletePatient(Patient patient);
        void UpdatePatient(Patient patient);
        bool Save();
        Task<bool> SaveAsync();
    }
}