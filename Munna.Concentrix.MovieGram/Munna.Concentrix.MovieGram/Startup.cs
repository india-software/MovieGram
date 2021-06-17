using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Munna.Concentrix.MovieGram.Data;
using Munna.Concentrix.MovieGram.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram
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
            services.AddDbContext<MovieGramDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MovieGramConnection")));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MovieMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<IMovieGramDbContext, MovieGramDbContext>();
           
            services.AddScoped(typeof(IMovieGramRepository<>), typeof(MovieGramRepository<>));
            services.AddScoped<IMovieService, MovieService>();

            services.AddControllers();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            });
            services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Concentrix API",
                Description = "Concentrix API",
                Contact = new OpenApiContact() { Name = "Munna Kumar Singh", Email = "singh@munna316@gmail.com" }
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This middleware serves generated Swagger document as a JSON endpoint
            app.UseSwagger();

            // This middleware serves the Swagger documentation UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Concentrix API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

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
