namespace Billing.Infrastructure.Seeders
{

    using AutoBogus;
    using Billing.Core.Entities;
    using Billing.Infrastructure.Contexts;
    using System.Linq;

    public static class InsuranceProviderSeeder
    {
        public static void SeedSampleInsuranceProviderData(BillingDbContext context)
        {
            if (!context.InsuranceProviders.Any())
            {
                context.InsuranceProviders.Add(new AutoFaker<InsuranceProvider>());
                context.InsuranceProviders.Add(new AutoFaker<InsuranceProvider>());
                context.InsuranceProviders.Add(new AutoFaker<InsuranceProvider>());

                context.SaveChanges();
            }
        }
    }
}