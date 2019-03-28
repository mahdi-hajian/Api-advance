using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Autorizes;
using Services.Interfaces;
using Services.UserService;
using System;
using WebFramework.Configuration;
using WebFramework.Middleware;

namespace MyApi
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddCorsExtention();

            // با این متد میشود این تنظیمات را داخل کانسترکتور ها دریافت کرد
            // مثال در کانسترکتور JWTSwrvice.cs
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddSbContext(Configuration);

            services.AddElmah(Configuration, _siteSetting);

            // ترتیب بین این دو مورد پایین مهم است
            services.AddCustomIdentity(_siteSetting.IdentitySettings);
            services.AddJwtAuthentication(_siteSetting.JwtSettings);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // برای استفاده از اتوفک
            // ابتدا خروجی این متد را از ووید به آی سرویس پروایدر تغییر میدهید و عملیات های زیر را انجام میدهیم
            return services.BuilderServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomExceptionHandler();

            app.UseHsts(env);

            //app.UseCors("SiteCorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseElmah();

            app.UseMvc();
        }
    }
}