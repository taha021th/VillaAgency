
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Domain.Entities.Users;
using VillaAgency.Infrastructure.Repositories;
using VillaAgency.Infrastructure.Services;
using VillaAgency.Infrastructure.Settings;

namespace VillaAgency.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {



            //Get data AppSetting
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
            var dbSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IVideoStorageService, VideoStorageService>();

            //Services
            services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IVisitRequestRepository, VisitRequestRepository>();
            services.AddScoped<IPropertySubmissionRepository, PropertySubmissionRepository>();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
                (
                    dbSettings.ConnectionString, dbSettings.DatabaseName
                );


            return services;
        }
    }
}