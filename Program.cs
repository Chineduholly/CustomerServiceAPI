global using CustomerServiceAPI.Models;
global using CustomerServiceAPI.Dto;
global using CustomerServiceAPI.Interfaces;
global using CustomerServiceAPI.Services;
global using CustomerServiceAPI.Context;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.EntityFrameworkCore;
global using System.Net;
global using Newtonsoft.Json;
using Serilog;
using Serilog.Events;


try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
      .Enrich.FromLogContext()
      .WriteTo.File(
        path: @"C:\\AppLogs\\CustomerServiceAPI\\log-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information,
        retainedFileCountLimit: 31
        ).CreateLogger();

    Log.Information($"Application is Starting....{DateTimeOffset.Now}");
    var ConnectionStr = builder.Configuration.GetConnectionString("DevConn");

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(ConnectionStr, options => options.EnableRetryOnFailure()));
    builder.Services.AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Failed to Start");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
