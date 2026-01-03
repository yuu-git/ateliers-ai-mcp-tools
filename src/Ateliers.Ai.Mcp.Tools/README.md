# Ateliers.Ai.Mcp.Tools

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/)

[Model Context Protocol (MCP)](https://modelcontextprotocol.io/)ツール実装のための基本インターフェースとモデルを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools`は、MCPツールを実装するための基底クラスと共通インフラストラクチャを提供します。このパッケージは、Ateliers AI MCPエコシステムにおけるすべてのツール実装の基盤となります。

## 🔧 主な機能

- **McpToolBase** - すべてのMCPツールの基底クラス
- **ロギング統合** - IMcpLoggerを使用した統一的なログ記録
- **共通インフラストラクチャ** - ツール実装に必要な共通機能

## 📚 使い方

### 基本的なツールの実装

```csharp
using Ateliers.Ai.Mcp.Tools;

public class MyCustomTool : McpToolBase
{
    public MyCustomTool() : base()
    {
    }

    public MyCustomTool(IMcpLogger mcpLogger) : base(mcpLogger)
    {
    }

    public async Task<string> ExecuteAsync()
    {
        McpLogger?.LogInfo("ツールを実行しています...");
        
        // ツールのロジックを実装
        
        return "実行完了";
    }
}
```

### ロギングの使用

```csharp
public class MyTool : McpToolBase
{
    public MyTool(IMcpLogger mcpLogger) : base(mcpLogger)
    {
    }

    public async Task ProcessAsync()
    {
        McpLogger?.LogDebug("処理を開始します");
        
        try
        {
            // 処理ロジック
            McpLogger?.LogInfo("処理が成功しました");
        }
        catch (Exception ex)
        {
            McpLogger?.LogError($"エラーが発生しました: {ex.Message}");
            throw;
        }
    }
}
```

## 🏗️ アーキテクチャ

### 基底クラス

**McpToolBase**
- すべてのMCPツールの共通基底クラス
- ロガーインスタンスの管理
- ツール実装の一貫性を保証

### 依存関係

- [Ateliers.Ai.Mcp.Core](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Core/) - コアインターフェースとユーティリティ
- [Ateliers.Ai.Mcp.Services](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Services/) - サービス層実装

## 🔗 関連パッケージ

Ateliers AI MCPエコシステムの一部として、以下の専門ツールパッケージを提供しています：

- **[Ateliers.Ai.Mcp.Tools.Notion](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Notion/)** - Notion統合ツール
- **[Ateliers.Ai.Mcp.Tools.Git](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Git/)** - Git操作ツール
- **[Ateliers.Ai.Mcp.Tools.Repository](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Repository/)** - リポジトリ管理ツール
- **[Ateliers.Ai.Mcp.Tools.Docusaurus](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus/)** - Docusaurusドキュメント管理ツール
- **[Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev/)** - ateliers.dev専用ツール
- **[Ateliers.Ai.Mcp.Tools.Presentation](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Presentation/)** - プレゼンテーション統合ツール

## 🌟 Ateliers AI MCP エコシステム

```
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Core                   │  基本インターフェース
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Services               │  サービス層
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Tools (このパッケージ) │  ツール基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  専門ツールパッケージ                     │  実装ツール
│  - Notion, Git, Repository, etc.        │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Processes (実行可能アプリケーション)      │  MCPサーバー
└─────────────────────────────────────────┘
```

## 📖 ドキュメント

完全なドキュメント、使用例、ガイドについては **[ateliers.dev](https://ateliers.dev)** をご覧ください。

## ⚠️ ステータス

このパッケージは開発中です（v0.x.x）。APIは予告なく変更される可能性があります。

## 📄 ライセンス

このプロジェクトはMITライセンスの下でライセンスされています - 詳細は[LICENSE](https://github.com/yuu-git/ateliers-ai-mcp-tools/blob/master/LICENSE)ファイルをご覧ください。

## 🔗 リンク

- **Website**: [ateliers.dev](https://ateliers.dev)
- **GitHub**: [yuu-git/ateliers-ai-mcp-tools](https://github.com/yuu-git/ateliers-ai-mcp-tools)
- **NuGet**: [Ateliers Packages](https://www.nuget.org/profiles/ateliers)
- **Documentation**: [ateliers.dev](https://ateliers.dev)

---

Made with ❤️ by [ateliers.dev](https://ateliers.dev)
