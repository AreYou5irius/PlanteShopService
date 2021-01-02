using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace PlanteShopRESTService
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
            services.AddControllers();

            //Swagger
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Plante API", Version = "v1.0" });
            });

            //Cors muligheder
            services.AddCors(options =>
            {

                //her giver vi alle lov til at tilgå server med alle metoder
                options.AddPolicy("AllowAnyOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

                //her giver vi alle lov til at bruge get og put på serveren
                options.AddPolicy("AllowAnyOriginGetPut", builder => builder.AllowAnyOrigin().WithMethods("GET", "PUT").AllowAnyHeader());

                //her giver vi tilladelse at alle der tilgår vores server via zealand.dk har lov til alt
                options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins("http://zealand.dk").AllowAnyMethod().AllowAnyHeader());

                //her giver vi tilladelse at alle der tilgår vores server via localhost:54905 har lov til alt
                options.AddPolicy("AllowMyLocalOrigin", builder => builder.WithOrigins("http://localhost:53852").AllowAnyMethod().AllowAnyHeader());

                //her giver vi tilladelse at alle der tilgår vores server via localhost:54905 har lov til at get og post
                options.AddPolicy("AllowLocalOriginGetPost", builder => builder.WithOrigins("http://localhost:53852").WithMethods("GET", "POST"));

            });

            //InMemory DB
            services.AddDbContext<PlanteContext>(opt => opt.UseInMemoryDatabase("ListOfPlants"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plante API v1.0")
            );

            app.UseRouting();

            //Cors benyttets
            app.UseCors("AllowAnyOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
