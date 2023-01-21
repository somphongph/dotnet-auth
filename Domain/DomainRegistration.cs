using System.Reflection;
using Domain.Services.Auth;
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

            #region Services
            services.AddScoped<IAuthService, AuthService>();
            #endregion


            return services;
        }
    }
}