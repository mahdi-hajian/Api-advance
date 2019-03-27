using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Autorizes;
using Services.Interfaces;
using WebFramework.Configuration;
using WebFramework.Middleware;

namespace MyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        private readonly SiteSettings _siteSetting;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // با این متد میشود این تنظیمات را داخل کانسترکتور ها دریافت کرد
            // مثال در کانسترکتور JWTSwrvice.cs
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddDbContext<ApplicationDbContext>(Options =>
            {
                Options.UseSqlServer((Configuration.GetConnectionString("DefaultConnection")));
            });


            services.AddElmah<SqlErrorLog>(options =>
            {
                options.Path = _siteSetting.ElmahPath;
                options.ConnectionString = Configuration.GetConnectionString("ElmahError");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJWTService, JWTService>();

            services.AddJwtAuthentication(_siteSetting.JwtSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomExceptionHandler();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // دوباره اکسپشن صادر میکند که بصورت جیسون هم دریافت شود
                //app.UseExceptionHandler();
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseAuthentication();

            // اگ بعد از اتورایز بزاریم فقط کسانی میتونن الماه رو ببینن که اتورایز شدن
            app.UseElmah();

            app.UseMvc();
        }
    }
}