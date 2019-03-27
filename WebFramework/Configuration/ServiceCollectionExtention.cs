using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public static class ServiceCollectionExtention
    {
        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false ;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // تلورانس زمان توکنه ک صفر میکنیم ک دقیق باشد
                    ClockSkew = TimeSpan.Zero, // default 5 min
                    RequireSignedTokens = true, // توکن ها حتما امضا داشته باشند
                    ValidateIssuer = true, // default false
                    ValidateAudience = true, // default false
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateLifetime = true, // زمان اکسپایر شدن رو بررسی کند یا ن
                    RequireExpirationTime = false, // حتما زمان اکسپایر شدن را داشته باشد
                    ValidateIssuerSigningKey = true, // سیگنیچر مورد بررسی قرار بگیرد
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
        }
    }
}
