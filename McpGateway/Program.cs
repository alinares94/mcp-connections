var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Middleware de logging para ver que el gateway recibe el tráfico
app.Use(async (context, next) =>
{
    Console.WriteLine($"[GATEWAY] {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"[GATEWAY] Respuesta: {context.Response.StatusCode}");
});

app.MapReverseProxy();

app.Run("http://localhost:5001");
