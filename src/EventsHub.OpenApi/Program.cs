using System.Reflection;
using EventsHub.Api.Controllers;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .AddOpenApiDocument(document =>
    {
        document.DocumentName = "EventsHub";
        document.Title = "EventsHubV1"; // Official interface name. No spaces. PascalCase.
        document.Version = "1.0.0";
        document.DefaultResponseReferenceTypeNullHandling =
            NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
    });

var pluginAssembly = Assembly.GetAssembly(typeof(WeatherForecastController));
services.AddMvc()
    .AddApplicationPart(pluginAssembly!)
    .AddControllersAsServices()
    .AddNewtonsoftJson(options =>
    {
        // Match the API's camelCase JSON output.
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

var app = builder.Build();

// Configura el middleware de NSwag especificando la ruta del documento
app.UseOpenApi(settings =>
{
    settings.Path = "/swagger/{documentName}/swagger.json";
});

app.UseSwaggerUi(settings =>
{
    settings.DocumentTitle = "EventsHubV1";
    settings.Path = "/swagger";
    settings.DocumentPath = "/swagger/EventsHub/swagger.json";
});

app.MapControllers();
app.Run();