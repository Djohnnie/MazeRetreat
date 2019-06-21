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
                c.SwaggerDoc("v2019", new Info
                {
                    Version = "Maze Retreat 2019",
                    Title = "Maze Retreat 2019",
                    Description = "This page provides some information about the June 2019 MazeRetreat for Involved!",
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
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2019/swagger.json", "Maze Retreat 2019");
            });

            app.UseMiddleware<RequestUriMiddleware>();
            app.UseMvc();
        }
    }
}