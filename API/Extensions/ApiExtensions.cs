using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                var securityDefinition = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authentication header using bearer scheme."
                };

                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDefinition);

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }
}
