using Common;
using EFSecondLevelCache.Core;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebFramework.Configuration;
using WebFramework.Configuration.Caching_configuraion_Extention;
using WebFramework.CustomMapping;
using WebFramework.Middleware;
using WebFramework.Swagger;

namespace MyApi
{ 
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AutoMapperConfiguration.InitializeAutoMapper();

            //Mapper.Initialize(config => {
            //    config.CreateMap<Post, PostDto>().ReverseMap()
            //    .ForMember(p=>p.Author, o=>o.Ignore())
            //    .ForMember(p=>p.Category, o=>o.Ignore());
            //});

            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEFSecondLevelCache();

            services.AddCorsExtention();

            services.AddCachingServiceExtention();

            // با این متد میشود این تنظیمات را داخل کانسترکتور ها دریافت کرد
            // مثال در کانسترکتور JWTSwrvice.cs
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddSbContext(Configuration);

            services.AddElmah(Configuration, _siteSetting);

            services.AddCustomApiVersioning();

            services.AddSwagger();

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

            app.UseElmah();

            app.UseCors("SiteCorsPolicy");

            app.UseSwaggerAndUI();

            app.UseEFSecondLevelCache();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}