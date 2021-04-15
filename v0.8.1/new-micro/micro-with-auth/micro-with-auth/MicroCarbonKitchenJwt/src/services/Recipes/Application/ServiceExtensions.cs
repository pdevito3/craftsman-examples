namespace Application
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        }
    }
}