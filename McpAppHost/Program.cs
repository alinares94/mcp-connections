var builder = DistributedApplication.CreateBuilder(args);

var mcpServer = builder.AddProject<Projects.McpServer>("mcp-server")
    .WithHttpEndpoint(port: 5000, isProxied: false);

var mcpGatewayYarp = builder.AddProject<Projects.McpGateway>("mcp-gateway-yarp")
    .WithHttpEndpoint(port: 5001, isProxied: false)
    .WaitFor(mcpServer);

var mcpGatewayOcelot = builder.AddProject<Projects.McpGatewayOcelot>("mcp-gateway-ocelot")
    .WithHttpEndpoint(port: 5003, isProxied: false)
    .WaitFor(mcpServer);

builder.AddProject<Projects.McpWebApp>("mcp-webapp")
    .WithHttpEndpoint(port: 5002, isProxied: false)
    .WaitFor(mcpGatewayYarp)
    .WaitFor(mcpGatewayOcelot);

builder.Build().Run();
