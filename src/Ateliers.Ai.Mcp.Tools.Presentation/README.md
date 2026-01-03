# Ateliers.Ai.Mcp.Tools.Presentation

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.Presentation.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Presentation/)

プレゼンテーション動画生成のためのModel Context Protocol (MCP)ツールを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Presentation
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools.Presentation`は、Markdownとナレーションテキストからプレゼンテーション動画を自動生成するMCPツールです。インターフェースベースの疎結合な設計により、様々な実装を柔軟に切り替えることができます。

## 🔧 主な機能

### プレゼンテーション動画生成
- **Markdown → スライド** - Markdown形式のテキストからスライド生成
- **テキスト → 音声** - ナレーションテキストから音声合成
- **スライド + 音声 → 動画** - スライドと音声を統合して動画を生成

### 特徴
- ✅ Markdown形式でスライドを記述
- ✅ 日本語ナレーション対応
- ✅ スライドごとの音声タイミング自動調整
- ✅ インターフェースによる疎結合設計
- ✅ 実装の柔軟な切り替えが可能

## 📚 使い方

### 基本的な使用例

```csharp
using Ateliers.Ai.Mcp.Tools.Presentation;

var presentationTool = new PresentationVideoTool(
    mcpLogger,
    presentationVideoGenerator
);

// Markdownとナレーションを用意
var markdown = @"
# MCPとは

Model Context Protocolの紹介

---

## 主な機能

- ツール連携
- リソース管理
- プロンプト管理
";

var narrations = new[]
{
    "エムシーピーとは、モデルコンテキストプロトコルのことです。",
    "主な機能として、ツール連携、リソース管理、プロンプト管理があります。"
};

// プレゼンテーション動画を生成
var result = await presentationTool.GeneratePresentationVideo(
    markdown,
    narrations
);
```

### 重要なナレーション記述ルール

**✅ 正しい記述（日本語）**
```csharp
var narrations = new[]
{
    "エムシーピーとは、モデルコンテキストプロトコルのことです。",
    "ブイエスコードで開発できます。",
    "ギットハブと連携します。"
};
```

**❌ 避けるべき記述（英語・記号）**
```csharp
var narrations = new[]
{
    "MCP とは、Model Context Protocol のことです。",  // 英語は音質低下の可能性
    "VS Code で開発できます。",                      // アルファベット表記は音声合成に影響
    "AI ・ MCP ・ Tool",                            // 記号は不自然な区切りの原因
};
```

### ナレーションテキストの変換例

| 元の表記 | ナレーション用 |
|---------|------------|
| MCP | エムシーピー |
| VS Code | ブイエスコード |
| GitHub | ギットハブ |
| AI Tool | エーアイツール |
| C# | シーシャープ |

### Markdownの記述

Markdown内は英語・アルファベット表記を使用できます（音声に影響しません）：

```markdown
# MCP とは

Model Context Protocolの紹介

---

## VS Code での開発

GitHub連携の手順
```

## 🏗️ アーキテクチャ

### 動画生成フロー

```
Markdown
    ↓
[スライド生成エンジン]
    ↓
スライド画像 + ナレーションテキスト
    ↓
[音声合成エンジン]
    ↓
音声ファイル
    ↓
[動画エンコーダー]
    ↓
プレゼンテーション動画
```

### インターフェース設計

このパッケージはインターフェースベースの疎結合設計を採用しており、各コンポーネントを自由に実装・置換できます：

- **IPresentationVideoGenerator** - 動画生成の統合インターフェース
- 実装は注入可能（Dependency Injection）
- 異なる技術スタックへの切り替えが容易

### 依存関係

- **[Ateliers.Ai.Mcp.Tools](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/)** - MCPツール基盤
- **[Ateliers.Ai.Mcp.Services](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Services/)** - プレゼンテーション生成サービス（インターフェース定義）
- **[ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/)** - 公式MCP SDK

## 💡 実装例

### デフォルト実装で使用されるツール

現在のデフォルト実装では、以下のツールを使用しています（これらは必須ではありません）

- **Marp CLI** - Markdownからスライド生成
- **VOICEVOX** - 日本語音声合成
- **FFmpeg** - 動画エンコーディング

### セットアップ例（デフォルト実装を使用する場合）

#### 1. VOICEVOXのインストール

```bash
# VOICEVOXをダウンロードしてインストール
# https://voicevox.hiroshiba.jp/
```

#### 2. Marp CLIのインストール

```bash
npm install -g @marp-team/marp-cli
```

#### 3. FFmpegのインストール

```bash
# Windows (Chocolatey)
choco install ffmpeg

# macOS (Homebrew)
brew install ffmpeg

# Linux (apt)
sudo apt install ffmpeg
```

### カスタム実装の例

独自の実装を使用する場合：

```csharp
// カスタム実装を作成
public class CustomPresentationGenerator : IPresentationVideoGenerator
{
    public async Task<PresentationVideoResult> GenerateAsync(
        PresentationVideoRequest request)
    {
        // 独自のスライド生成ロジック
        // 独自の音声合成ロジック
        // 独自の動画統合ロジック
        
        return new PresentationVideoResult
        {
            VideoPath = outputPath
        };
    }
}

// カスタム実装を注入
var customGenerator = new CustomPresentationGenerator();
var presentationTool = new PresentationVideoTool(
    mcpLogger,
    customGenerator  // カスタム実装を使用
);
```

## 🎬 使用例

### 技術解説動画の生成

```csharp
var markdown = @"
---
marp: true
theme: default
---

# MCPサーバーの作り方

---

## 環境構築

- .NET 8以上
- Visual Studio 2022

---

## 実装手順

1. プロジェクト作成
2. パッケージインストール
3. ツール実装
";

var narrations = new[]
{
    "エムシーピーサーバーの作り方について説明します。",
    "環境構築には、ドットネット8以上と、ビジュアルスタジオ2022が必要です。",
    "実装手順は、プロジェクト作成、パッケージインストール、ツール実装の順に進めます。"
};

var result = await presentationTool.GeneratePresentationVideo(
    markdown,
    narrations
);

Console.WriteLine($"動画が生成されました: {result}");
```

## 🔗 関連パッケージ

### エコシステム

```
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Core                   │  基本インターフェース
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Services               │  サービス層（インターフェース）
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Tools                  │  ツール基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  このパッケージ                          │  プレゼンテーションツール
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
