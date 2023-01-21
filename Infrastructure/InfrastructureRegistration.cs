using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region SqlDB
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());
            #endregion

            #region MongoDb
            services.Configure<MongoSettings>(
                configuration.GetSection(nameof(MongoSettings)));

            services.AddScoped<IMongoSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoSettings>>().Value);
            #endregion

            #region Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("RedisSettings:ConnectionString").Value;
                options.InstanceName = configuration.GetSection("RedisSettings:InstanceName").Value;
            });
            #endregion

            #region Repositories dependency injection
            services.AddScoped<IAccountRepository, AccountRepository>();
            #endregion

            #region MongoDB Repositories dependency injection
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IAuthenMongoRepository, AuthenMongoRepository>();
            #endregion

            return services;
        }
    }
}