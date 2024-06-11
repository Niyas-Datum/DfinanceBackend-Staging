
using Dfinance.Shared.Configuration;
using Dfinance.AuthAppllication.Middlewares;
using Dfinance.api.Installers.ext;
using Serilog;
using Dfinance.AuthApplication.Middlewares;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Dfinance.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.InstallerServiceInAssembly(configuration);

// Logfile

//var logFilePath = "\\WEB-SERVER\\DfinaceBackEnd\\Project\\log\\.json";

string currentPath = Directory.GetCurrentDirectory();

if (!Directory.Exists(Path.Combine(currentPath, "Archive\\LogFile")))

    Directory.CreateDirectory(Path.Combine(currentPath, "Archive\\LogFile"));

var logFilePath = Path.Combine(currentPath, "Archive\\LogFile\\.json");

var logger = new LoggerConfiguration()

    .ReadFrom.Configuration(configuration)

    .Enrich.FromLogContext()

    .Filter.ByExcluding(e => e.MessageTemplate.Text.Contains("Now listening on:") ||

                                      e.MessageTemplate.Text.Contains("Application started. Press Ctrl+C to shut down.") ||

                                      e.MessageTemplate.Text.Contains("Hosting environment:") ||

                                      e.MessageTemplate.Text.Contains("Content root path:"))

    .WriteTo.File(

        new LogFormatterService(),

        logFilePath,

        rollingInterval: RollingInterval.Day)

    .CreateLogger();



Log.Logger = logger;

builder.Logging.AddSerilog(logger);

// Add AutoMapper to the service collection


var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperConfig());
});

IMapper mapper = mapperConfig.CreateMapper();
services.AddSingleton(mapper);

//builder.Services.AddAutoMapper(typeof(Program).Assembly);
var app = builder.Build();
// Logfile
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.File("D:\\UserFiles\\UPAS\\CommonWorkingProject(Upas)\\Dfinance\\Dfinance.AuthApplication\\LogFile\\logfile.txt")
//    .CreateLogger();

// swagger 
var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(nameof(swaggerOptions)).Bind(swaggerOptions);
app.UseSwagger(options => { options.RouteTemplate = swaggerOptions.JsonRoute; });
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(swaggerOptions.UiEndPoint, swaggerOptions.Description);
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


// global cors policy
//app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

//Middleware - authentication checking - every request
app.UseMiddleware<JwtMiddleware>();

//Middleware - Logging
app.UseMiddleware<LogMiddleware>(logFilePath);

app.MapControllers();

app.Run();
