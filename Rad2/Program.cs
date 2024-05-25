using Append.Blazor.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Rad2.Areas.Identity;
using Rad2.Data;
using Rad2.Models.Domian;
using Rad2.Policy;
using Rad2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<dbContext>();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Contoso");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CertifiedMinimumHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CrudpHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, NameHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("HRPolicy", policy => policy.RequireClaim("HR"));
    options.AddPolicy("AtLeast18", policy =>
      policy.Requirements.Add(new MinimumAgeRequirement(18)));
    options.AddPolicy("IsExpertCertifiedAnd18", policy =>
     policy.Requirements.Add(new CertifiedMinimumRequirement(true, 5)));
    options.AddPolicy("Crudp", policy =>
     policy.Requirements.Add(new CrudpRequirement(true, true, true, true, true)));
    options.AddPolicy("Name", policy =>
    policy.Requirements.Add(new NameRequirement("*")));
});

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseAssignmentService, CourseAssignmentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IOfficeAssignmentService, OfficeAssignmentService>();
builder.Services.AddScoped<ITitlesService, TitlesService>();

builder.Services.AddScoped<IPrintingService, PrintingService>();


var app = builder.Build();

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
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
