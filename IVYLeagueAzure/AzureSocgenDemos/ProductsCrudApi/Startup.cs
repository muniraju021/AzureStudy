using Azure.Storage.Blobs;
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
using ProductsCrudApi.Data;
using ProductsCrudApi.Repository;
using ProductsCrudApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductsCrudApi
{
    public class Startup
    {
        private const string AccessKey = "AccessKey";
        private const string BlobQueueStorage = "BlobQueueStorage";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));
            services.AddScoped<IProductBrandRepository, ProductBrandRepository>();
            services.AddScoped<IProductItemRepository, ProductItemRepository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ProductsCrudApiSpec", new OpenApiInfo {
                    Title = "Products API",
                    Version = "1",
                    Description = "Products Crud API",
                    Contact = new OpenApiContact
                    {
                        Email = "muniraju021@gmail.com",
                        Name = "Muniraju JAYARAMA",
                        Url = new Uri("http://localhost:8090")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MJ License",
                        Url = new Uri("https://wwww.linkedin.com/in/muniraju021")
                    }
                });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(cmlCommentsFullFilePath);
            });

            string blobQueueStorage = Configuration[BlobQueueStorage];
            services.Add(new ServiceDescriptor(typeof(BlobServiceClient), new BlobServiceClient(blobQueueStorage)));
            services.AddTransient<IBlobService, BlobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ProductsCrudApiSpec/swagger.json", "Products API");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
