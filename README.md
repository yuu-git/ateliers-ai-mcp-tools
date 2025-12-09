# Ateliers AI Model Context Protocol (MCP) Tools

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)

MCP tool implementations for [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) in C#.

## Packages

```bash
# Base tool interfaces
dotnet add package Ateliers.Ai.Mcp.Tools

# Notion MCP tools
dotnet add package Ateliers.Ai.Mcp.Tools.Notion

# Git MCP tools
dotnet add package Ateliers.Ai.Mcp.Tools.Git

# Repository management tools
dotnet add package Ateliers.Ai.Mcp.Tools.Repository

# Docusaurus integration tools
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus

# Ateliers.dev specific Docusaurus tools
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev
```

## Features

- **Tools** - Base interfaces and models for MCP tool implementation
- **Notion** - MCP tools for Notion integration (tasks, ideas, reading lists)
- **Git** - MCP tools for Git operations (pull, push, commit, tag)
- **Repository** - MCP tools for repository management (Git, GitHub, local filesystem)
- **Docusaurus** - MCP tools for Docusaurus documentation management
- **Docusaurus.AteliersDev** - Ateliers.dev-specific Docusaurus tool implementations

## Dependencies

All packages depend on:
- [Ateliers.Ai.Mcp.Core](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Core/) - Core library
- [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/) - Official MCP SDK

## Ateliers AI MCP Ecosystem

- **Core** - Base interfaces and utilities
- **Services** - Service layer implementations
- **Tools** (this package) - MCP tool implementations
- **Servers** - MCP server implementations

## Documentation

Visit **[ateliers.dev](https://ateliers.dev)** for full documentation, usage examples, and guides.

## Status

⚠️ **Development version (v0.x.x)** - API may change. Stable v1.0.0 coming soon.

## License

MIT License - see [LICENSE](LICENSE) file for details.

---

**[ateliers.dev](https://ateliers.dev)** | **[GitHub](https://github.com/yuu-git/ateliers-ai-mcp-tools)** | **[NuGet](https://www.nuget.org/profiles/ateliers)**
