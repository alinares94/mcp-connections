using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public class HelloTools
{
    [McpServerTool, Description("Saluda al usuario por su nombre")]
    public static string SayHello([Description("Nombre del usuario")] string name, [Description("Número de veces a repetir el saludo")] int repeat = 1)
    {
        var greeting = $"¡Hola, {name}! Mensaje desde el MCP Server (a través del Gateway).";
        return string.Join("\n", Enumerable.Repeat(greeting, repeat));
    }
}
