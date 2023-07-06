using GridMvc.Demo.Filters;
using GridMvc.Demo.Models;
using GridMvc.Demo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ra.Models.Domian;
using RadShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using GridShared.Data;
using GridMvc;
using RadShared.Data;
using Ra.Services;
using Blazored.Modal;

namespace Ra
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NorthwindDbContext>(options =>
            {
                options.UseGridBlazorDatabase();
                //options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
            });

 /*           
                        services.AddDbContext<ChinookDbContext>(options =>
                        {
                            options.UseGridBlazorDatabase2();
                            //options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
                        });
 */           

            services.AddDbContext<MyDbContext>();

            // Add framework services.
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddGridMvc();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization()
;
//                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddScoped<LanguageFilter>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddBlazoredModal();

            services.Configure<RequestLocalizationOptions>(
                 options =>
                 {
                     var supportedCultures = new List<CultureInfo>
                         {
                            new CultureInfo("en-US"),
                            new CultureInfo("de-DE"),
                            new CultureInfo("it-IT"),
                            new CultureInfo("es-ES"),
                            new CultureInfo("fr-FR"),
                            new CultureInfo("ru-RU"),
                            new CultureInfo("nb-NO"),
                            new CultureInfo("nl-NL"),
                            new CultureInfo("tr-TR"),
                            new CultureInfo("cs-CZ"),
                            new CultureInfo("sl-SI"),
                            new CultureInfo("se-SE"),
                            new CultureInfo("sr-Cyrl-RS"),
                            new CultureInfo("hr-HR"),
                            new CultureInfo("fa-IR"),
                            new CultureInfo("ca-ES"),
                            new CultureInfo("gl-ES"),
                            new CultureInfo("eu-ES"),
                            new CultureInfo("pt-BR"),
                            new CultureInfo("bg-BG")
                         };

                     options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                     options.SupportedCultures = supportedCultures;
                     options.SupportedUICultures = supportedCultures;
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
