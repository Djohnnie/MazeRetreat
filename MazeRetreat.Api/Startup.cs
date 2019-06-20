using System;
using System.IO;
using MazeRetreat.Api.DataAccess;
using MazeRetreat.Api.Helpers;
using MazeRetreat.Api.Logic;
using MazeRetreat.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MazeRetreat.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            String xmlFile = "MazeRetreat.Api.xml";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Maze Retreat 2019", new Info
                {
                    Version = "Maze Retreat 2019",
                    Title = "Maze Retreat 2019",
                    Description = "This page provides some documentation on the available endpoints for the Hack The Future 2018 .NET Challenge.",
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            });

            services.AddDbContext<DatabaseContext>();
            services.AddTransient<MazeLogic>();
            services.AddTransient<RenderingLogic>();
            services.AddTransient<ImageLogic>();
            services.AddScoped<MazeRetreatContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/swagger.json", "Maze Retreat 2019");
            });

            app.UseMiddleware<RequestUriMiddleware>();
            app.UseMvc();
        }
    }
}