using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using music_manager_starter.Data;
using Serilog;
using System.Security.AccessControl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Serilog Logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Host.UseSerilog();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


using (var scope = app.Services.CreateScope())
{

    var path = "App_Data";

    if (!Path.Exists(path)) Directory.CreateDirectory(path);
    var dbContext = scope.ServiceProvider.GetRequiredService<DataDbContext>();


    dbContext.Database.Migrate();

}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
