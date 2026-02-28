using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

Console.WriteLine("=== MCP Client ===");
Console.WriteLine("Conectando al Gateway (YARP) en http://localhost:5001 ...\n");

// En 1.0.0 el cliente HTTP usa SseClientTransport para conectarse via HTTP/SSE
// El cliente se conecta al GATEWAY, no al server directamente
var clientTransport = new HttpClientTransport(new HttpClientTransportOptions
{
    Endpoint = new Uri("http://localhost:5001/api/mcp/analysis"),
    Name = "McpClient"
});

await using var client = await McpClient.CreateAsync(clientTransport);

Console.WriteLine("✅ Conectado al MCP Server (via Gateway YARP)\n");

// Listar herramientas disponibles
var tools = await client.ListToolsAsync();
Console.WriteLine("Herramientas disponibles:");
foreach (var tool in tools)
{
    Console.WriteLine($"  - {tool.Name}: {tool.Description}");
}

// Llamar a la herramienta SayHello
Console.WriteLine("\nLlamando a 'SayHello' con nombre 'Mundo'...");
var result = await client.CallToolAsync("say_hello", new Dictionary<string, object?>
{
    ["name"] = "Mundo"
});

Console.WriteLine("\nRespuesta del servidor:");
foreach (var content in result.Content)
{
    if (content.Type == "text")
        Console.WriteLine($"  👉 {content}");
}

Console.WriteLine("\n✅ ¡Hola Mundo MCP completado!");
