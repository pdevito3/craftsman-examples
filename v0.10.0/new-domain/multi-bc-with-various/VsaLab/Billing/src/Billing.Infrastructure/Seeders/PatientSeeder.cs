namespace Billing.Infrastructure.Seeders
{

    using AutoBogus;
    using Billing.Core.Entities;
    using Billing.Infrastructure.Contexts;
    using System.Linq;

    public static class PatientSeeder
    {
        public static void SeedSamplePatientData(BillingDbContext context)
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