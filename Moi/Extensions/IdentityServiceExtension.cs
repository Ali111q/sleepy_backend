using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GaragesStructure.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("Api", new OpenApiInfo
                {
                    Title = "Api Doc",
                    Version = "v1"
                });
                
                option.SwaggerDoc("DriverApp", new OpenApiInfo
                {
                    Title = "Driver App Doc (For Allawi)",
                    Version = "v1" 
                });
                
                
                option.SwaggerDoc("CommissionApp", new OpenApiInfo
                {
                    Title = "Commission App Doc (For Allawi)",
                    Version = "v1" 
                });
                

                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // option.IncludeXmlComments(xmlPath);


                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                
                option.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (apiDesc.ActionDescriptor.EndpointMetadata.OfType<ApiExplorerSettingsAttribute>().Any())
                    {
                        var settings = apiDesc.ActionDescriptor.EndpointMetadata.OfType<ApiExplorerSettingsAttribute>().First();
                        return settings.GroupName == docName;
                    }
                    return true;
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });


            return services;
        }
        





    }
}