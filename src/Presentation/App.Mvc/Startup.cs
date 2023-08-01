using App.Infra.Bootstrap;
using App.Infra.Bootstrap.Extensions;
using App.Infra.CrossCutting.Extensions;
using App.Mvc.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace App.Mvc
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private Container _container = Ioc.RecoverContainer();

        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration.UpdateFromEnvironment();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.UseBootstrap();

            #region Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Access/Login";
                        options.AccessDeniedPath = "/Home/Error/";
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    });
            #endregion

            #region Sessão
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {                  
                options.Cookie.Name = "WSIDS";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            #endregion

            #region Culture

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var enCultureInfo = new CultureInfo("en");

                enCultureInfo.NumberFormat.NumberDecimalSeparator = ".";

                var supportedCultures = new[]
                {
                    enCultureInfo,
                };

                options.DefaultRequestCulture = new RequestCulture(culture: enCultureInfo, uiCulture: enCultureInfo);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
            #endregion
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
                app.UseHsts();
            }

            app.UseBootstrap();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseRequestLocalization();

            app.UseSession();            

            app.UseMiddleware<AuthMiddleware>(_container)
               .UseAuthentication()
               .UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapFallback(context => {                              
                    context.Response.Redirect($"/Home/Error");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
