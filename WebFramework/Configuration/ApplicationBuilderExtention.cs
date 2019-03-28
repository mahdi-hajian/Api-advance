using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public static class ApplicationBuilderExtention
    {
        public static void UseHsts(this IApplicationBuilder app, IHostingEnvironment env)
        {
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
        }
    }
}
