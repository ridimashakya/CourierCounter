using CourierCounter.Services.Interfaces;
using CourierCounter.Services;
using CourierCounter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CourierCounter.Models.Entities;
using FluentValidation.AspNetCore;
using FluentValidation;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.Validator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<RegistrationViewModel>, RegistrationValidator>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWorkerServices, WorkerServices>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();

builder.WebHost.UseUrls("http://192.168.102.101:5183");

//inject ApplicationDbContext here after making ConnectionStrings in appsettings.json file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//configure identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
