namespace Ordering.Infrastructure.Seeders
{

    using AutoBogus;
    using Ordering.Core.Entities;
    using Ordering.Infrastructure.Contexts;
    using System.Linq;

    public static class PatientSeeder
    {
        public static void SeedSamplePatientData(OrderingDbContext context)
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