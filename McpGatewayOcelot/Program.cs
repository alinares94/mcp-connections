using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"[GATEWAY-OCELOT] {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"[GATEWAY-OCELOT] Respuesta: {context.Response.StatusCode}");
});

await app.UseOcelot();

await app.RunAsync("http://localhost:5003");
