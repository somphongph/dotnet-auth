using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DomainRegistration
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            #region MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());
            #endregion

            #region Filters
            // services.AddSingleton<ServiceAuthenFilter>();
            #endregion

            return services;
        }
    }
}