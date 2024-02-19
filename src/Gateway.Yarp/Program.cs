using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
   .AllowAnyMethod()
   .AllowAnyHeader()
   .AllowCredentials()
   .WithOrigins("http://localhost:4200")
   .SetIsOriginAllowedToAllowWildcardSubdomains()
   .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
   .WithHeaders("Authorization", "Content-Type", "x-requested-with");
}));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapReverseProxy();

app.Run();