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
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .UseResponseCaching();

app.MapControllers();

app.Services.SeedAsync().GetAwaiter().GetResult();
app.Run();