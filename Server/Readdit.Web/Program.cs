using Newtonsoft.Json;
using Readdit.Infrastructure;
using Readdit.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddResponseCaching()
    .AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseCors(options =>
        options
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials())
    .UseAuthentication()
    .UseAuthorization()
    .UseResponseCaching();

app.MapControllers();

app.Services.SeedAsync().GetAwaiter().GetResult();
app.Run();