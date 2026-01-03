# Ateliers.Ai.Mcp.Tools.Notion

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.Notion.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Notion/)

Notion統合のためのModel Context Protocol (MCP)ツールを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Notion
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools.Notion`は、NotionのTasks、Ideas、Reading Listを管理するためのMCPツールです。AIアシスタントと連携して、タスク管理、アイデア記録、読書リスト管理を効率化します。

## 🔧 主な機能

### タスク管理 (Tasks)
- **追加** (`AddTask`) - 新しいタスクを作成
- **更新** (`UpdateTask`) - タスク情報を変更
- **一覧** (`ListTasks`) - タスクをフィルタして表示
- **完了** (`CompleteTask`) - タスクを完了状態に

### アイデア管理 (Ideas)
- **追加** (`AddIdea`) - アイデアや構想を記録
- **検索** (`SearchIdeas`) - キーワードやタグで検索
- **更新** (`UpdateIdea`) - アイデアを更新

### 読書リスト (Reading List)
- **追加** (`AddToReadingList`) - 記事、書籍、動画などを登録
- **一覧** (`ListReadingList`) - 読書リストを表示
- **完了** (`UpdateReadingListStatus`) - 読了状態に更新

### 特徴
- ✅ 日本語対応（日本語のステータス、優先度）
- ✅ 詳細なメタデータ管理
- ✅ AI登録元の追跡（Claude/GPT/Copilot）
- ✅ 柔軟なフィルタリング

## 📚 使い方

### タスク管理の例

```csharp
using Ateliers.Ai.Mcp.Tools.Notion;

var notionTools = new NotionTools(
    mcpLogger,
    notionTasksService,
    notionIdeasService,
    notionReadingListService
);

// タスクを追加
var result = await notionTools.AddTask(
    title: "Phase 7の実装",
    description: "新機能の実装作業",
    status: "進行中",
    priority: "高",
    dueDate: DateTime.Now.AddDays(7),
    tags: new[] { "開発", "MCP" },
    registrant: "Claude"
);

// タスク一覧を取得
var tasks = await notionTools.ListTasks(
    status: "未着手",
    priority: "高",
    limit: 10
);

// タスクを完了
var completed = await notionTools.CompleteTask("task-id");
```

### アイデア管理の例

```csharp
// アイデアを追加
var ideaResult = await notionTools.AddIdea(
    title: "MCPサーバーの新機能",
    content: "複数のAIアシスタント間でコンテキストを共有する機能",
    tags: new[] { "MCP", "アイデア" },
    registrant: "GPT"
);

// アイデアを検索
var ideas = await notionTools.SearchIdeas(
    keyword: "MCP",
    tags: new[] { "技術" },
    limit: 10
);
```

### 読書リストの例

```csharp
// 読書リストに追加
var readingResult = await notionTools.AddToReadingList(
    title: "Model Context Protocol完全ガイド",
    link: "https://example.com/mcp-guide",
    type: "記事",
    priority: "高",
    tags: new[] { "MCP", "技術記事" },
    registrant: "Copilot"
);

// 読書リストを表示
var readingList = await notionTools.ListReadingList(
    status: "未読",
    priority: "高",
    limit: 20
);

// 読了にする
var updateResult = await notionTools.UpdateReadingListStatus(
    "reading-id",
    "完了",
    DateTime.Now
);
```

### MCPツールとしての使用

**AddTask**
```
WHEN TO USE:
- 「タスクを追加」「TODO追加」と言われた時
- 作業項目を記録したい時
- 期限付きのタスクを管理したい時

EXAMPLES:
✓ 'Phase 7の実装をタスクに追加して'
✓ '明日までにドキュメント作成、優先度高でタスク追加'
```

**AddIdea**
```
WHEN TO USE:
- アイデアや構想を記録したい時
- ひらめきをメモしたい時
- 技術的なアイデアを保存したい時

EXAMPLES:
✓ 'MCPサーバーの新機能アイデアをメモ'
✓ 'ビジネスアイデア：AI活用した営業支援ツール'
```

**AddToReadingList**
```
WHEN TO USE:
- 技術記事、書籍、動画を「あとで読む」リストに追加したい時
- 学習資料を管理したい時

EXAMPLES:
✓ 'このMCP記事、いつか読みたいからリーディングリストに追加'
✓ 'この技術書、優先度高で追加して'
```

## 🏗️ アーキテクチャ

### 依存関係

- **[Ateliers.Ai.Mcp.Tools](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/)** - MCPツール基盤
- **[Ateliers.Ai.Mcp.Services](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Services/)** - Notion サービス層
- **[ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/)** - 公式MCP SDK

### Notionデータベース構造

**Tasks**
- タイトル、説明、ステータス（未着手/進行中/完了/保留）
- 優先度（高/中/低）、期限、場所
- タグ、登録元（AI種別）

**Ideas**
- タイトル、内容、タグ
- ステータス、関連リンク、登録元

**Reading List**
- タイトル、URL、種類（記事/書籍/動画/論文）
- ステータス（未読/完了）、優先度
- 期限/開催日時、タグ、読後メモ

## 🔗 関連パッケージ

### エコシステム

```
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Core                   │  基本インターフェース
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Services               │  Notionサービス層
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Tools                  │  ツール基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  このパッケージ                          │  Notionツール
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
