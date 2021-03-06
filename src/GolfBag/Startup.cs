﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using GolfBag.Services;
using Microsoft.AspNetCore.Routing;
using GolfBag.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace GolfBag
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                if (_env.IsProduction())
                {
                    //config.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<ScoreCardDbContext>();

            services.AddSingleton(_config);
            services.AddSingleton<IGreeter, Greeter>();
            services.AddScoped<IRoundOfGolf, RoundOfGolfRepository>();
            services.AddIdentity<User, IdentityRole>(x =>
                {
                    x.Password.RequireDigit = false;
                    x.Password.RequiredLength = 5;
                    x.Password.RequireLowercase = false;
                    x.Password.RequireNonAlphanumeric = false;
                    x.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ScoreCardDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseFileServer();

            app.UseNodeModules(env.ContentRootPath);

            app.UseIdentity();

            app.UseMvc(ConfigureRoutes);           
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
