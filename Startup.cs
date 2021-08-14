using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.Web.Sitemap;

namespace SnipBucket.XSiteMap.Demo
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
            services.AddControllersWithViews();
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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            List<string> urlListToMap = new List<string>();
            urlListToMap.Add("https://www.snipbucket.org/snip/4/How-to-reload-a-nginx-server-in-ubuntu");
            urlListToMap.Add("https://www.snipbucket.org/snip/2/How-to-restart-a-redis-server-in-ubuntu");
            urlListToMap.Add("https://www.snipbucket.org/snip/1/Check-supported-TLS-version-in-ubuntu");
            GenerateXSiteMap(env, urlListToMap);
        }

        private void GenerateXSiteMap(IWebHostEnvironment env, List<string> urlList)
        {
            var siteMapFileSaveLocation = @$"{env.WebRootPath}/sitemap.xml";
            var xSiteMap = new Sitemap();

            // Add home page url in site map first
            xSiteMap.Add(new Url
            {
                ChangeFrequency = ChangeFrequency.Daily,
                Location = "https://www.snipbucket.org",
                Priority = 0.5,
                TimeStamp = DateTime.Now
            });

            //Add other urls
            foreach (var url in urlList)
            {
                xSiteMap.Add(CreateUrl(url));
            }
            xSiteMap.Save(siteMapFileSaveLocation);
        }
        private Url CreateUrl(string url)
        {
            return new Url
            {
                ChangeFrequency = ChangeFrequency.Daily,
                Location = url,
                Priority = 0.5,
                TimeStamp = DateTime.Now
            };
        }
    }
}
