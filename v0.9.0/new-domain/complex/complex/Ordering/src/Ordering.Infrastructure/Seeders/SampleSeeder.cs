namespace Ordering.Infrastructure.Seeders
{

    using AutoBogus;
    using Ordering.Core.Entities;
    using Ordering.Infrastructure.Contexts;
    using System.Linq;

    public static class SampleSeeder
    {
        public static void SeedSampleSampleData(OrderingDbContext context)
        {
            if (!context.Samples.Any())
            {
                context.Samples.Add(new AutoFaker<Sample>());
                context.Samples.Add(new AutoFaker<Sample>());
                context.Samples.Add(new AutoFaker<Sample>());

                context.SaveChanges();
            }
        }
    }
}