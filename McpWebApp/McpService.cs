using ModelContextProtocol.Client;
using System.Text.Json;

namespace McpWebApp;

public class McpService
{
    public string GatewayUrl { get; set; } = "http://localhost:5001/api/mcp/analysis";

    public async Task<List<ToolInfo>> ListToolsAsync()
    {
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri(GatewayUrl),
            Name = "McpWebApp"
        });
        await using var client = await McpClient.CreateAsync(transport);
        var tools = await client.ListToolsAsync();

        return tools.Select(t =>
        {
            JsonElement? schema = t.JsonSchema.ValueKind != JsonValueKind.Undefined
                ? t.JsonSchema
                : null;
            return new ToolInfo(t.Name, t.Description, schema);
        }).ToList();
    }

    public async Task<List<string>> CallToolAsync(string toolName, Dictionary<string, object?> args)
    {
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri(GatewayUrl),
            Name = "McpWebApp"
        });
        await using var client = await McpClient.CreateAsync(transport);
        var result = await client.CallToolAsync(toolName, args);
        return result.Content
            .Where(c => c.Type == "text")
            .Select(c => c.ToString() ?? string.Empty)
            .ToList();
    }
}
