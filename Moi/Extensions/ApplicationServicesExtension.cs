using AutoMapper;
using BackEndStructuer.Services;
using e_parliament.Interface;
using Microsoft.EntityFrameworkCore;
using GaragesStructure.DATA;
using GaragesStructure.Helpers;
using GaragesStructure.Repository;
using GaragesStructure.Services;

using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using RestSharp;

namespace GaragesStructure.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));


            services.AddFluentValidationRulesToSwagger();


            services.AddHangfire((sp, config) =>
            {
                var connection = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
                config.UsePostgreSqlStorage(connection);
            });


            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<AuthorizeActionFilter>();
            services.AddScoped<ICountryService, CountryService>();


            // here to add
services.AddScoped<IMusicServices, MusicServices>();
services.AddScoped<ICategoryServices, CategoryServices>();
       
            services.AddScoped<IFileService, FileService>();
           

            services.AddScoped<PermissionSeeder>();
            services.AddScoped<RoleSeeder>();
           
            services.AddScoped<IOtpCodeServices, OtpCodeServices>();
            services.AddScoped<INotificationProvider, NotificationProvider>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<IRestRequest, RestRequest>();
            
            
            // seed data from permission seeder service

            var serviceProvider = services.BuildServiceProvider();


            var permissionSeeder = serviceProvider.GetService<PermissionSeeder>();
            permissionSeeder.SeedPermissions();

            var roles = serviceProvider.GetService<RoleSeeder>();
            roles.AddRole();


            return services;
        }
    }
}
