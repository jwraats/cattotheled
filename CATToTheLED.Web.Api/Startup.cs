using CATToTheLED.Web.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CATToTheLED.Web.Api
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
            // This defines the class before we get started with the API
            // So this will clean the LED strip
            var _neopixel = NeoPixelStatic.Neopixel;

            //And... API magic
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "CATToTheLED", Version = "v1"});
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CATToTheLED Api Docs");
                c.RoutePrefix = "docs";
            });

            app.UseMvc();
        }
    }
}