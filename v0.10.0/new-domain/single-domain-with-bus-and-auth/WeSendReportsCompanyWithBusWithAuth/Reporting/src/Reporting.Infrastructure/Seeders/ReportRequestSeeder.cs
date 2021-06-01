namespace Reporting.Infrastructure.Seeders
{

    using AutoBogus;
    using Reporting.Core.Entities;
    using Reporting.Infrastructure.Contexts;
    using System.Linq;

    public static class ReportRequestSeeder
    {
        public static void SeedSampleReportRequestData(ReportingDbContext context)
        {
            if (!context.ReportRequests.Any())
            {
                context.ReportRequests.Add(new AutoFaker<ReportRequest>());
                context.ReportRequests.Add(new AutoFaker<ReportRequest>());
                context.ReportRequests.Add(new AutoFaker<ReportRequest>());

                context.SaveChanges();
            }
        }
    }
}