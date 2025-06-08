using FCSP.WebAPI;
using FCSP.WebAPI.Configuration;
using Microsoft.AspNetCore.Builder;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Azure;
using FCSP.Services.PaymentService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var config = builder.Configuration;

config.AddUserSecrets<Program>();

var services = builder.Services;

services.AddControllers();
builder.Services.AddHostedService<VoucherExpirationService>();
builder.Services.AddHostedService<DesignerManufacturerPaymentService>();
builder.Services.AddHostedService<PaymentTimeoutService>();
builder.Services.AddLogging(logging => logging.AddConsole());
services.AddHttpClient();
services.AddEndpointsApiExplorer();



ServiceConfig.Configure(services);
RepositoryConfig.Configure(services);
AuthConfig.Configure(services, config);
DocumentationConfig.Configure(services);
CorsConfig.Configure(services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Redirect to Swagger UI when accessing the root URL
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
