# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the entire solution
dotnet build

# Recommended: launch everything with .NET Aspire (respects startup order)
cd McpAppHost && dotnet run

# Manual: start services in dependency order (3+ terminals)
cd McpServer && dotnet run         # http://localhost:5000  (start first)
cd McpGateway && dotnet run        # http://localhost:5001  (after server)
cd McpGatewayOcelot && dotnet run  # http://localhost:5003  (after server)
cd McpWebApp && dotnet run         # http://localhost:5002  (after gateways)

# Console client (alternative to McpWebApp)
cd McpClient && dotnet run
```

There are no automated tests in this project.

## Architecture

This is a .NET 10 demonstration of the **Model Context Protocol (MCP)** with a mandatory gateway layer. Clients never communicate directly with the MCP server — all traffic routes through a gateway.

```
Browser → McpWebApp :5002 → Gateway :5001/:5003 → McpServer :5000
McpClient                 → Gateway :5001/:5003 → McpServer :5000
```

**Key rule:** `McpClient` and `McpWebApp` connect to `http://localhost:5001` (YARP) or `http://localhost:5003` (Ocelot), never directly to `:5000`.

### Projects

| Project | Role | Port |
|---|---|---|
| `McpServer` | Exposes MCP tools via HTTP/SSE transport | 5000 |
| `McpGateway` | YARP reverse proxy; routes `/api/mcp/*` → server `/mcp/*` | 5001 |
| `McpWebApp` | Blazor Web UI; lets users discover and invoke tools | 5002 |
| `McpGatewayOcelot` | Alternative Ocelot gateway with identical routing | 5003 |
| `McpClient` | Console client for headless testing | — |
| `McpAppHost` | .NET Aspire orchestrator; enforces startup order and health checks | — |

### MCP Implementation Details

- **Server (`McpServer/`):** Uses `ModelContextProtocol.AspNetCore`. Tools are defined as static methods decorated with `[McpServerTool]` and `[Description]` attributes in `HelloTools.cs`. Auto-discovered via `.WithToolsFromAssembly()`.
- **Gateways:** Both YARP (`appsettings.json`) and Ocelot (`ocelot.json`) perform the same path rewrite. YARP adds request/response logging middleware.
- **Web client (`McpWebApp/McpService.cs`):** Creates an `HttpClientTransport` pointing at the selected gateway endpoint. `Home.razor` lets the user toggle between YARP and Ocelot at runtime.
- **Transport:** HTTP + SSE (Server-Sent Events) — no WebSocket or gRPC.
- **Aspire (`McpAppHost/Program.cs`):** Defines explicit `.WaitFor()` dependencies: gateways wait for `McpServer`, `McpWebApp` waits for both gateways. All ports are fixed (`isProxied: false`). Running via Aspire is the recommended approach because it enforces this startup order automatically.

### Adding a New Tool

Add a static method to `McpServer/HelloTools.cs` (or a new class in `McpServer/`) decorated with:

```csharp
[McpServerTool, Description("Tool description")]
public static string MethodName([Description("param description")] string param) { ... }
```

No registration required — `.WithToolsFromAssembly()` discovers it automatically.
