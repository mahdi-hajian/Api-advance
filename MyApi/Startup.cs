using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CacheManager.Core;
using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using EFSecondLevelCache.Core;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services.Autorizes;
using Services.Interfaces;
using Services.Models.Dtos;
using Services.UserService;
using System;
using WebFramework.Caching;
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

            Mapper.Initialize(config => {
                config.CreateMap<Post, PostDto>().ReverseMap()
                .ForMember(p=>p.Author, o=>o.Ignore())
                .ForMember(p=>p.Category, o=>o.Ignore());
            });

            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEFSecondLevelCache();

            services.AddCorsExtention();

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

            app.UseEFSecondLevelCache();

            app.UseCors("SiteCorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseElmah();

            app.UseMvc();
        }
    }
}