using FCSP.WebAPI;
using FCSP.WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

config.AddUserSecrets<Program>();

var services = builder.Services;

services.AddControllers();
builder.Services.AddHostedService<VoucherExpirationService>();
builder.Services.AddLogging(logging => logging.AddConsole());
services.AddHttpClient();
services.AddEndpointsApiExplorer();

ServiceConfig.Configure(services);
RepositoryConfig.Configure(services);
AuthConfig.Configure(services, config);
DocumentationConfig.Configure(services);

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

// if (app.Environment.IsDevelopment())
// {
    
// }

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
