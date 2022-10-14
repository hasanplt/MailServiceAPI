using HasanPolatCom.Application.Helpers.JWT;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationRegistration(this IServiceCollection services, string privateKey)
        {
            var ass = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(ass);
            services.AddMediatR(ass);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {


                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Hasan.Security.Bearer",
                    ValidAudience = "Hasan.Security.Bearer",
                    IssuerSigningKey =
                            JwtSecurityKey.Create(privateKey)
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context => {
                        Console.WriteLine("OnAuthenticationFailed :" + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context => {
                        Console.WriteLine("OnTokenValidated :" + context.SecurityToken);
                        return Task.CompletedTask;
                    },
                };
            });


            services.AddAuthorization(options => {
                options.AddPolicy("Member", policy => policy.RequireClaim("MembershipId"));
            });
        }
    }
}
