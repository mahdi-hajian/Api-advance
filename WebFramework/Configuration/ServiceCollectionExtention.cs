using Common;
using Common.Utilities;
using Data;
using Data.Contracts;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace WebFramework.Configuration
{
    public static class ServiceCollectionExtention
    {
        public static void AddSbContext(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddDbContext<ApplicationDbContext>(Options =>
            {
                Options.UseSqlServer((Configuration.GetConnectionString("DefaultConnection")));
            });
        }

        public static void AddElmah(this IServiceCollection services, IConfiguration Configuration, SiteSettings _siteSetting)
        {
            services.AddElmah<SqlErrorLog>(options =>
            {
                options.Path = _siteSetting.ElmahPath;
                options.ConnectionString = Configuration.GetConnectionString("ElmahError");
            });
        }

        public static void AddCorsExtention(this IServiceCollection services)
        {
            // add "ClientDomain": "https://www.tabandesign.ir" in appsetting.json
            // add app.UseCors("SiteCorsPolicy"); in Configure method

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            //corsBuilder.WithOrigins("https://www.tabandesign.ir"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            corsBuilder.AllowCredentials();
            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });
        }

        public static void AddMinimalMvc(this IServiceCollection services)
        {
            services.AddMvcCore()
            .AddApiExplorer()
            .AddAuthorization()
            .AddFormatterMappings()
            .AddDataAnnotations()
            .AddJsonFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        bool UpdateLastLogin = true;
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                        {
                            context.Fail("This token has no secuirty stamp");
                            UpdateLastLogin = false;
                        }

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token secuirty stamp is not valid.");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                        {
                            context.Fail("Token secuirty stamp is not valid.");
                            UpdateLastLogin = false;
                        }
                        if (user != null)
                        {
                            if (!user.IsActive)
                            {
                                context.Fail("User is not active.");
                                UpdateLastLogin = false;
                            }
                            if (UpdateLastLogin)
                                await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                        }
                    }
                };
            });
        }
    }
}
