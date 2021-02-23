namespace Infrastructure.Shared
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            // add shared services service like a DateTimeService, EmailService, MessagingService, etc.
        }
    }
}
