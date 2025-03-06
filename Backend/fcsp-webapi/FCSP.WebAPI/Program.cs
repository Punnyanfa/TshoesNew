using FCSP.WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

config.AddUserSecrets<Program>();

var services = builder.Services;

services.AddControllers();
services.AddHttpClient();
services.AddEndpointsApiExplorer();

ServiceConfig.Configure(services);
RepositoryConfig.Configure(services);

AuthConfig.Configure(services, config);

DocumentationConfig.Configure(services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
