﻿using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class NopMvcStartup : INopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            //add MiniProfiler services
            services.AddNopMiniProfiler();

            //add WebMarkupMin services to the services container
            services.AddNopWebMarkupMin();

            //add and configure MVC feature
            services.AddNopMvc();

            //add custom redirect result executor
            services.AddNopRedirectResultExecutor();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseSwagger();
            application.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Nop.Web.index.html");
            });
            //use MiniProfiler
            application.UseMiniProfiler();

            //use WebMarkupMin
            application.UseNopWebMarkupMin();

            //MVC routing
            application.UseNopMvc();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 1000; //MVC should be loaded last
    }
}