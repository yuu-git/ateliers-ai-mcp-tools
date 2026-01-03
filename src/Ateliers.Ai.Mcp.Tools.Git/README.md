# Ateliers.Ai.Mcp.Tools.Git

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.Git.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Git/)

Git操作のためのModel Context Protocol (MCP)ツールを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Git
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools.Git`は、Gitリポジトリの操作（pull、commit、push、tag管理）をMCPツールとして提供します。複数のリポジトリを統一的なインターフェースで管理でき、AI支援による Git ワークフローの自動化を実現します。

## 🔧 主な機能

### 基本操作
- **Pull** (`PullRepository`) - リモートから最新の変更を取得
- **Commit** (`CommitRepository`) - ローカルの変更をコミット
- **Push** (`PushRepository`) - コミットをリモートにプッシュ
- **Commit & Push** (`CommitAndPushRepository`) - コミットとプッシュを一度に実行

### タグ管理
- **Tag作成** (`CreateTag`) - 軽量タグまたは注釈付きタグを作成
- **Tagプッシュ** (`PushTag`) - タグをリモートにプッシュ
- **Tag作成 & プッシュ** (`CreateAndPushTag`) - タグ作成とプッシュを一度に実行

### 特徴
- ✅ 複数リポジトリの統一管理
- ✅ 詳細なエラーハンドリング
- ✅ マージコンフリクトの検出
- ✅ コミットハッシュの追跡

## 📚 使い方

### 基本的な使用例

```csharp
using Ateliers.Ai.McpServer.Tools;

var gitTools = new GitTools(mcpLogger, gitService);

// リモートから最新を取得
var pullResult = await gitTools.PullRepository("AteliersAiMcpServer");

// 変更をコミット
var commitResult = await gitTools.CommitRepository(
    "AteliersAiMcpServer",
    "Phase 5: Add Git integration"
);

// リモートにプッシュ
var pushResult = await gitTools.PushRepository("AteliersAiMcpServer");

// コミット＆プッシュを一度に
var result = await gitTools.CommitAndPushRepository(
    "AteliersAiMcpServer",
    "Update documentation"
);
```

### タグ管理の例

```csharp
// リリースタグを作成
var tagResult = await gitTools.CreateTag(
    "AteliersAiMcpServer",
    "v0.5.0",
    "Phase 5 complete"
);

// タグをリモートにプッシュ
var pushTagResult = await gitTools.PushTag(
    "AteliersAiMcpServer",
    "v0.5.0"
);

// タグ作成とプッシュを一度に
var createAndPushResult = await gitTools.CreateAndPushTag(
    "AteliersAiMcpServer",
    "v1.0.0",
    "Version 1.0.0 release"
);
```

### MCPツールとしての使用

**PullRepository**
```
WHEN TO USE:
- 作業開始前に最新の変更を取得する時
- 他の人がプッシュした変更を同期する時
- コミット前にリモートと同期する時

EXAMPLES:
✓ 'Pull latest changes from AteliersAiMcpServer'
✓ 'Sync PublicNotes repository with remote'
```

**CommitAndPushRepository**
```
WHEN TO USE:
- 完了した作業をすぐに共有したい時
- 小さな変更を迅速に反映したい時
- 単一の論理的な作業単位を公開する時

EXAMPLES:
✓ 'Commit and push AteliersAiMcpServer with message Phase 5 complete'
✓ 'Save and publish changes: Update documentation'
```

**CreateAndPushTag**
```
WHEN TO USE:
- リリースポイントをマークする時
- マイルストーンを作成して共有する時
- 重要なコミットにタグを付ける時

EXAMPLES:
✓ 'Create and push tag v0.5.0 with message Phase 5 complete'
✓ 'Tag and publish milestone phase-6-start'
```

## 🏗️ アーキテクチャ

### 依存関係

- **[Ateliers.Ai.Mcp.Tools](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/)** - MCPツール基盤
- **[Ateliers.Ai.Mcp.Services](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Services/)** - Git サービス層
- **[ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/)** - 公式MCP SDK

### 対応リポジトリ

デフォルトで以下のリポジトリキーに対応：
- `AteliersAiAssistants` - コーディングガイドラインとトレーニングサンプル
- `AteliersAiMcpServer` - MCPサーバーソースコード
- `AteliersDev` - 技術記事とブログ投稿
- `PublicNotes` - TODO、アイデア、スニペット
- `TrainingMcpServer` - トレーニング用MCPサーバーコード

## 🔗 関連パッケージ

### 関連ツール

- **[Ateliers.Ai.Mcp.Tools.Repository](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Repository/)** - リポジトリファイル管理ツール

### エコシステム

```
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Core                   │  基本インターフェース
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Services               │  サービス層（GitService）
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Tools                  │  ツール基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  このパッケージ                          │  Git操作ツール
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
