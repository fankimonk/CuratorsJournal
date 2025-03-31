using Application.Interfaces;
using Application.Services;
using Application.Authorization;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

        //Old
        //public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //        {
        //            options.TokenValidationParameters = new()
        //            {
        //                ValidateIssuer = false,
        //                ValidateAudience = false,
        //                ValidateLifetime = true,
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
        //            };
                    
        //            options.Events = new JwtBearerEvents
        //            {
        //                OnMessageReceived = context =>
        //                {
        //                    context.Token = context.Request.Cookies["tasty-cookies"];
        //                    return Task.CompletedTask;
        //                }
        //            };
        //        });

        //    services.AddScoped<IPermissionsService, PermissionsService>();
        //    services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        //    services.AddAuthorization(options =>
        //    {
        //        options.AddPolicy("UserPolicy", policy =>
        //        {
        //            policy.AddRequirements(new PermissionRequirement([Permissions.Logout]));
        //        });
        //    });
        //}
    }
}
