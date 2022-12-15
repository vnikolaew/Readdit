using Readdit.Infrastructure;
using Readdit.Services;
using Readdit.Web.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddInfrastructure(configuration)
    .AddApplication(configuration)
    .AddWebComponents();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseCors(options =>
        options
            .WithOrigins(configuration["CorsOrigins"].Split(";"))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials())
    .UseAuthentication()
    .UseAuthorization()
    .UseResponseCaching();

app.MapControllers();
app.Services.SeedAsync().GetAwaiter().GetResult();

app.Run();