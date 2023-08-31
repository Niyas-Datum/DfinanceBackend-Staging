
using Dfinance.Shared.Configuration;
using Dfinance.AuthAppllication.Authorization;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthAppllication.Middlewares;
using Dfinance.api.Installers.ext;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.InstallerServiceInAssembly(configuration);

var app = builder.Build();

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


app.MapControllers();

app.Run();
