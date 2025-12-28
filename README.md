# Ateliers AI Model Context Protocol (MCP) Tools

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)

C#による[Model Context Protocol (MCP)](https://modelcontextprotocol.io/)のMCPツール実装です。

## パッケージ

```bash
# 基本ツールインターフェース
dotnet add package Ateliers.Ai.Mcp.Tools

# Notion MCP ツール
dotnet add package Ateliers.Ai.Mcp.Tools.Notion

# Git MCP ツール
dotnet add package Ateliers.Ai.Mcp.Tools.Git

# リポジトリ管理ツール
dotnet add package Ateliers.Ai.Mcp.Tools.Repository

# Docusaurus 統合ツール
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus

# ateliers.dev 専用 Docusaurus ツール
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev

# プレゼンテーション統合ツール
dotnet add package Ateliers.Ai.Mcp.Tools.Presentation
```

## 機能

- **Tools** - MCPツール実装のための基本インターフェースとモデル
- **Notion** - Notion統合のためのMCPツール（タスク、アイデア、読書リスト）
- **Git** - Git操作のためのMCPツール（pull、push、commit、tag）
- **Repository** - リポジトリ管理のためのMCPツール（Git、GitHub、ローカルファイルシステム）
- **Docusaurus** - Docusaurusドキュメント管理のためのMCPツール
- **Docusaurus.AteliersDev** - Ateliers.dev専用のDocusaurusツール
- **Presentation** - プレゼンテーション動画統合のためのMCPツール

## 依存関係

すべてのパッケージは以下に依存しています：
- [Ateliers.Ai.Mcp.Core](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Core/) - コアライブラリ
- [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/) - 公式MCP SDK

## Ateliers AI MCP エコシステム

- **Core** - MCPエコシステム全ての基本インターフェースとユーティリティ
- **Services**（このパッケージ）- サービス層実装
- **Tools** - 複数の MCP サービスを組み合わせた MCP タスク単位の実装
- **processes** - 各アプリケーション向けの MCP ツール実行ファイル（.exe）

## ドキュメント

完全なドキュメント、使用例、ガイドについては **[ateliers.dev](https://ateliers.dev)** をご覧ください。

## ステータス

⚠️ **開発版 (v0.x.x)** - APIは変更される可能性があります。安定版v1.0.0は近日リリース予定です。

## ライセンス

MITライセンス - 詳細は[LICENSE](LICENSE)ファイルをご覧ください。

---

**[ateliers.dev](https://ateliers.dev)** | **[GitHub](https://github.com/yuu-git/ateliers-ai-mcp-tools)** | **[NuGet](https://www.nuget.org/profiles/ateliers)**
