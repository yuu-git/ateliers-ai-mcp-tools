# Ateliers.Ai.Mcp.Tools.Repository

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.Repository.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Repository/)

リポジトリ管理のためのModel Context Protocol (MCP)ツールを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Repository
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools.Repository`は、Git、GitHub、ローカルファイルシステムを統一的に管理するMCPツールです。ファイルの読み取り、作成、編集、削除など、リポジトリ操作を効率化します。

## 🔧 主な機能

### ファイル操作
- **読み取り** (`ReadRepositoryFile`) - ローカルまたはGitHubからファイルを読み取る
- **一覧** (`ListRepositoryFiles`) - ディレクトリ内のファイルを一覧表示
- **作成** (`AddRepositoryFile`) - 新しいファイルを作成
- **編集** (`EditRepositoryFile`) - 既存ファイルを更新（自動バックアップ）
- **削除** (`DeleteRepositoryFile`) - ファイルを削除（自動バックアップ）

### ファイル管理
- **リネーム** (`RenameRepositoryFile`) - ファイル名変更または移動
- **コピー** (`CopyRepositoryFile`) - ファイルを複製
- **バックアップ** (`BackupRepositoryFile`) - 手動バックアップ作成

### 特徴
- ✅ ローカル優先、GitHubフォールバック
- ✅ 自動バックアップ機能
- ✅ 複数リポジトリの統一管理
- ✅ 拡張子によるフィルタリング
- ✅ Git操作との連携

## 📚 使い方

### 基本的な使用例

```csharp
using Ateliers.Ai.McpServer.Tools;

var repositoryTools = new RepositoryTools(
    mcpLogger,
    gitHubService,
    localFileService,
    gitService
);

// ファイルを読み取る
var content = await repositoryTools.ReadRepositoryFile(
    "AteliersAiMcpServer",
    "README.md"
);

// ファイル一覧を取得
var files = await repositoryTools.ListRepositoryFiles(
    "AteliersAiMcpServer",
    directory: "Services",
    extension: ".cs"
);

// 新しいファイルを作成
var addResult = await repositoryTools.AddRepositoryFile(
    "AteliersAiMcpServer",
    "Services/NewService.cs",
    "// New service implementation"
);

// ファイルを編集（自動バックアップ）
var editResult = await repositoryTools.EditRepositoryFile(
    "AteliersAiMcpServer",
    "README.md",
    "# Updated README content"
);
```

### ファイル管理の例

```csharp
// ファイルをリネーム
var renameResult = await repositoryTools.RenameRepositoryFile(
    "AteliersAiMcpServer",
    "OldService.cs",
    "Services/NewService.cs"
);

// ファイルをコピー
var copyResult = await repositoryTools.CopyRepositoryFile(
    "AteliersAiMcpServer",
    "template.md",
    "new-document.md",
    overwrite: false
);

// バックアップを作成
var backupResult = await repositoryTools.BackupRepositoryFile(
    "AteliersAiMcpServer",
    "appsettings.json",
    "2024-11-26"
);

// ファイルを削除（自動バックアップ）
var deleteResult = await repositoryTools.DeleteRepositoryFile(
    "AteliersAiMcpServer",
    "test.txt"
);
```

### MCPツールとしての使用

**ReadRepositoryFile**
```
WHEN TO USE:
- ソースコードファイルを読み取る時
- ドキュメントやMarkdownファイルを確認する時
- 設定ファイルを調べる時

EXAMPLES:
✓ 'Read Services/GitHubService.cs from AteliersAiMcpServer'
✓ 'Show me README.md from AteliersAiAssistants'
```

**EditRepositoryFile**
```
WHEN TO USE:
- 既存のソースコードを修正する時
- ドキュメントを更新する時
- バグを修正する時

NOTE: 自動的にバックアップが作成されます

EXAMPLES:
✓ 'Update README.md in AteliersAiMcpServer with new content'
✓ 'Fix bug in Services/GitHubService.cs'
```

**ListRepositoryFiles**
```
WHEN TO USE:
- リポジトリ構造を探索する時
- 特定のファイルを探す時
- 拡張子でフィルタする時

EXAMPLES:
✓ 'List all markdown files in AteliersAiAssistants'
✓ 'Show C# files in AteliersAiMcpServer Services directory'
```

## 🏗️ アーキテクチャ

### 優先順位

```
ファイル操作リクエスト
    ↓
ローカルファイルシステム（優先）
    ↓（利用不可の場合）
GitHub API（フォールバック）
```

### 依存関係

- **[Ateliers.Ai.Mcp.Tools](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/)** - MCPツール基盤
- **[Ateliers.Ai.Mcp.Services](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Services/)** - ファイル・GitHubサービス
- **[ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/)** - 公式MCP SDK

### 対応リポジトリ

デフォルトで以下のリポジトリキーに対応：
- `AteliersAiAssistants` - コーディングガイドラインとトレーニングサンプル
- `AteliersAiMcpServer` - MCPサーバーソースコード
- `AteliersDev` - 技術記事とブログ投稿
- `PublicNotes` - TODO、アイデア、スニペット
- `TrainingMcpServer` - トレーニング用MCPサーバーコード

## 💡 Git連携について

このパッケージはファイル操作のみを提供します。Git操作（commit、push、pull）は別パッケージで行います：

```csharp
// 1. ファイルを編集
await repositoryTools.EditRepositoryFile(
    "AteliersAiMcpServer",
    "README.md",
    "# Updated content"
);

// 2. Git操作は GitTools で実行
await gitTools.CommitAndPushRepository(
    "AteliersAiMcpServer",
    "Update README"
);
```

## 🔗 関連パッケージ

### 関連ツール

- **[Ateliers.Ai.Mcp.Tools.Git](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Git/)** - Git操作ツール
- **[Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev/)** - 記事専用ツール（Frontmatter除去）

### エコシステム

```
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Core                   │  基本インターフェース
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Services               │  ファイル・GitHubサービス
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Tools                  │  ツール基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  このパッケージ                          │  リポジトリ管理ツール
└─────────────────────────────────────────┘
```

## 📖 ドキュメント

完全なドキュメント、使用例、ガイドについては **[ateliers.dev](https://ateliers.dev)** をご覧ください。

## 🚧 開発ステータス

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
