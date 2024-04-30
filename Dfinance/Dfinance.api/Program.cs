
using Dfinance.Shared.Configuration;
using Dfinance.AuthAppllication.Middlewares;
using Dfinance.api.Installers.ext;
using Serilog;
using Dfinance.AuthApplication.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.InstallerServiceInAssembly(configuration);

// Logfile

var logFilePath = "D:\\UserFiles\\VIJAL\\GitHubProjectForWork\\Dfinance\\Dfinance.AuthApplication\\Archive\\LogFile\\.json";

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.File(
        new LogFormatterService(),
        logFilePath,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Logger = logger;
builder.Logging.AddSerilog(logger);


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
