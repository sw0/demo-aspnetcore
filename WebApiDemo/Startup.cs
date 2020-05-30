using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApiDemo.Filters;
using WebApiDemo.Services;

namespace WebApiDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddXmlDataContractSerializerFormatters()
                //.AddXmlSerializerFormatters() //https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.1
                .ConfigureApiBehaviorOptions(options =>
                {
                    //https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#automatic-http-400-responses
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddMvcOptions(options =>
                {
                    //
                    //options.RespectBrowserAcceptHeader = true;

                    options.Filters.Add<ValidateModelActionFilterAttribute>();
                    options.Filters.Add<ValidateModelAsyncActionFilterAttribute>();

                    options.Filters.Add<AddAuthorHeaderResultAttribute>();
                });

            services.AddScoped<IAuthorService, AuthorService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Poem API",
                    Version = "v1",
                    Description = "A sample ASPNET Core WebAPI application"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Poem API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
