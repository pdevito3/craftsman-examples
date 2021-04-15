namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class PatientSeeder
    {
        public static void SeedSamplePatientData(LabDbContext context)
        {
            if (!context.Patients.Any())
            {
                context.Patients.Add(new AutoFaker<Patient>());
                context.Patients.Add(new AutoFaker<Patient>());
                context.Patients.Add(new AutoFaker<Patient>());

                context.SaveChanges();
            }
        }
    }
}