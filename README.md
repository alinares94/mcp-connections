# MCP Hola Mundo con YARP Gateway

## Arquitectura

```
McpClient  ──────────────────────────────────┐
                                              ↓
Browser → McpWebApp (5002) → McpGateway/YARP (5001) → McpServer (5000)
```

El cliente y la web app **nunca hablan directamente con el server**, siempre pasan por el gateway.

## Cómo ejecutar

### Opción A — Web UI (recomendado)

Necesitas **3 terminales**:

```bash
# Terminal 1 – MCP Server
cd McpServer && dotnet run    # http://localhost:5000

# Terminal 2 – Gateway YARP
cd McpGateway && dotnet run   # http://localhost:5001

# Terminal 3 – Web App
cd McpWebApp && dotnet run    # http://localhost:5002
```

Luego abre **http://localhost:5002** en el navegador, haz clic en **Load Tools** y llama a las herramientas desde la interfaz.

### Opción B — Cliente de consola

Necesitas **3 terminales**:

```bash
# Terminal 1 – MCP Server
cd McpServer && dotnet run

# Terminal 2 – Gateway YARP
cd McpGateway && dotnet run

# Terminal 3 – MCP Client
cd McpClient && dotnet run
```

## Salida esperada del cliente

```
=== MCP Client ===
Conectando al Gateway (YARP) en http://localhost:5001 ...

✅ Conectado al MCP Server (via Gateway YARP)

Herramientas disponibles:
  - SayHello: Saluda al usuario por su nombre

Llamando a 'SayHello' con nombre 'Mundo'...

Respuesta del servidor:
  👉 ¡Hola, Mundo! Mensaje desde el MCP Server (a través del Gateway).

✅ ¡Hola Mundo MCP completado!
```

## En el Gateway verás logs como:
```
[GATEWAY] POST /sse
[GATEWAY] Respuesta: 200
```

## Estructura del proyecto

```
McpSolution/
├── McpServer/          ← Servidor MCP con tool "SayHello"
│   ├── Program.cs
│   ├── HelloTools.cs
│   └── McpServer.csproj
├── McpGateway/         ← Reverse proxy YARP
│   ├── Program.cs
│   ├── appsettings.json
│   └── McpGateway.csproj
├── McpClient/          ← Cliente consola
│   ├── Program.cs
│   └── McpClient.csproj
└── McpWebApp/          ← Web UI para probar el servidor
    ├── Program.cs
    ├── McpWebApp.csproj
    └── wwwroot/
        └── index.html
```

## Paquetes NuGet usados

- `ModelContextProtocol` 1.0.0 — SDK oficial de MCP para .NET
- `ModelContextProtocol.AspNetCore` 1.0.0 — integración ASP.NET Core
- `Yarp.ReverseProxy` 2.2.0 — Gateway/proxy inverso de Microsoft
