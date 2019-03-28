using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services.Autorizes;
using Services.Interfaces;
using Services.UserService;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public static class AutofacConfiguraionExtection
    {
        public static void AddService(this ContainerBuilder containerBuilder)
        {
            // Register type > As(example interface) > lifteme
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            // روش دستی
            //containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<JWTService>().As<IJWTService>().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            // AssemblyScannign + Auto/Conventianal Registraion------روش اتوماتیک
            // برای دریافت اسمبلی باید یه کلاس و ... که درون اون اسمبلیه رو تایپش رو بگیریم
            var commonAssembly = typeof(SiteSettings).Assembly;
            var EntitiesAssembly = typeof(IEntity).Assembly;
            var DataAssembly = typeof(ApplicationDbContext).Assembly;
            var ServicesAssembly = typeof(JWTService).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, EntitiesAssembly, DataAssembly, ServicesAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, EntitiesAssembly, DataAssembly, ServicesAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, EntitiesAssembly, DataAssembly, ServicesAssembly)
                .AssignableTo<ISingeltonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        // جایگزین کردن سرویس پرووایدر اتوفک بجای سرویس پرووایدر پیشفرض مایکروسافت
        public static IServiceProvider BuilderServiceProvider(this IServiceCollection Service)
        {
            // روش قبلی خود مایکروسافت
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped<IUserRepository, UserRepository>();


            var containerBuilder = new ContainerBuilder();

            // سرویس هایی که قبلا اضافه شده است رو به اتوفک اضافه میکند
            containerBuilder.Populate(Service);

            // register services to autofac containerBuilder
            containerBuilder.AddService();

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}

// قابلیت هایی که اتوفک دارد ولی سرویس پروایدر مایکی ندارد
// PropertyInjection
// AssemblyScannign + Auto/Conventianal Registraion
// Inerception
