using ASPNetCoreWithCAP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNetCoreWithCAP
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Register Subscribers
            services.AddTransient<SubscriberService>();
            // Register CAP configurations
            services.AddCap(x =>
            {
                // Register CAP Dashboard
                x.UseDashboard();

                // sql server configure
                x.UseSqlServer(options =>
                {
                    options.ConnectionString = "Data Source=U2-LATITUDE;Initial Catalog=PlayGroundCore;User ID=sa;Password=Lunasea1212";
                });

                // rabbit configure
                x.UseRabbitMQ(options =>
                {
                    options.HostName = "192.168.227.131";
                    options.Port = 5672;
                    options.VirtualHost = "host";
                    options.ExchangeName = "DefaultExchange";
                    options.UserName = "admin";
                    options.Password = "Lunasea2019";
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
