using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExploreCalifornia
{
    public class Startup
    {
        public readonly IConfigurationRoot configuration;
        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder()
             .AddEnvironmentVariables()
             .AddJsonFile(env.ContentRootPath + "/config.json")
             .AddJsonFile(env.ContentRootPath + "/config.developer.json", true)
             .Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperException = configuration.GetValue<bool>("FeatureToggle:EnableDeveloperException")
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, FeatureToggles featureToggles)
        {
            
            app.UseExceptionHandler("/error.html");

            //var configuaration = new ConfigurationBuilder()
            //    .AddEnvironmentVariables()
            //    .AddJsonFile(env.ContentRootPath + "/config.json")
            //    .AddJsonFile(env.ContentRootPath + "/config.developer.json",true)
            //    .Build();
            //if (env.IsDevelopment())
            //if(configuaration.GetValue<bool>("FeatureToggle:EnableDeveloperException"))
            if(featureToggles.EnableDeveloperException)
            {
                app.UseDeveloperExceptionPage();
            }            

            app.Use(async (context, next) =>
            {                
                if(context.Request.Path.Value.Contains(@"invalid"))
                    throw new Exception("Error!");
                await next();
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World ");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default",
                    "{controller=Home}/{action=Index}/{id?}"
                    );
            });
            app.UseFileServer();
        }
    }
}
