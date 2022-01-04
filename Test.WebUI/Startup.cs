using Adverthouse.Common.Data;
using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.Data.ElasticSearch;
using Adverthouse.Common.Data.MongoDB;
using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using Adverthouse.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Test.WebUI.Models.Services;

namespace Test.WebUI
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
           

            var settings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton<AppSettings>(settings);

            Singleton<AppSettings>.Instance = settings;

            JToken jAppSettings = JToken.Parse(
                  File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json"))
            );

            settings.AdditionalSettings = (IDictionary<string,JToken>)jAppSettings["AppSettings"]["AdditionalSettings"];
            
            var root = settings.AdditionalSettings["SitemapRootAddres"].Value<string>();


            string temp = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json"));
            var additionalSettings = JsonConvert.DeserializeObject<AppSettings>(temp);

            ///       
            /// repositories                      
            services.AddTransient(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddSingleton(typeof(IElasticRepository<>), typeof(ElasticRepository<>));
            services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            services.AddSingleton<ICacheManager<MemoryCacheManager>, MemoryCacheManager>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = false;
                options.MaximumBodySize = 1024;
                options.SizeLimit = 100 * 1024 * 1024;
            });

            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile
                {
                    Duration = 60
                });

                options.CacheProfiles.Add("test", new CacheProfile
                {
                    Duration = 30
                });
            });

            services.AddMvc()
              .AddControllersAsServices()
              .AddRazorRuntimeCompilation();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseResponseCaching(); // response caching


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
