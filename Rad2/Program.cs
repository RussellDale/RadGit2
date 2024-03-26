using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Rad2.Areas.Identity;
using Rad2.Models.Domian;
using Rad2.Data;
using Rad2.Services;
using Append.Blazor.Printing;
using GridMvc;
using GridMvc.Demo.Filters;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using GridMvc.Demo.Models;
using GridMvc.Demo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RadShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridShared.Data;
using RadShared.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Contoso");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<dbContext>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddGridMvc();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMvc()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization()
;

builder.Services.AddScoped<LanguageFilter>();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseAssignmentService, CourseAssignmentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IOfficeAssignmentService, OfficeAssignmentService>();
builder.Services.AddScoped<ITitlesService, TitlesService>();
builder.Services.AddScoped<IPrintingService, PrintingService>();


builder.Services.Configure<RequestLocalizationOptions>(
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

var app = builder.Build();

//var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
//app.UseRequestLocalization(locOptions.Value);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
