using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductCatalog API",
        Version = "v1",
        Description = "Homework API for products, status codes, Swagger, Scalar, and Postman testing."
    });
});

var app = builder.Build();

app.UseSwagger(options =>
{
    options.RouteTemplate = "openapi/{documentName}.json";
});

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/openapi/v1.json", "ProductCatalog API v1");
});

app.MapScalarApiReference("/scalar", options =>
{
    options
        .WithTitle("ProductCatalog API")
        .WithOpenApiRoutePattern("/openapi/{documentName}.json");
});

app.MapGet("/", () => Results.Redirect("/scalar"))
    .ExcludeFromDescription();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
