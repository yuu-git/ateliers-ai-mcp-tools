# Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev/)

[ateliers.dev](https://ateliers.dev)専用のDocusaurus技術記事管理MCPツールを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev`は、ateliers.devの技術記事やブログ投稿を効率的に管理するための専用MCPツールです。Frontmatterの自動除去、記事検索、一覧表示など、技術文書作成に特化した機能を提供します。

## 🔧 主な機能

### 記事管理
- **記事読み取り** (`ReadArticle`) - Frontmatterを自動除去してMarkdown本文のみを取得
- **記事一覧** (`ListArticles`) - docsやblog内の全記事を一覧表示（.md / .mdx対応）
- **記事検索** (`SearchArticles`) - キーワードによる記事検索とプレビュー

### 特徴
- ✅ Frontmatter（YAMLメタデータ）の自動除去
- ✅ Markdown (.md) と MDX (.mdx) の両形式対応
- ✅ キーワードマッチによるコンテキスト表示
- ✅ GitHub経由での記事アクセス

## 📚 使い方

### 基本的な使用例

```csharp
using Ateliers.Ai.Mcp.Tools.Docusaurus;

var tools = new AteliersDevTools(mcpLogger, gitHubService);

// 技術記事を読み取る
var article = await tools.ReadArticle(
    "docs/csharp/datetime-extensions.md"
);

// 記事を検索する
var searchResults = await tools.SearchArticles(
    keyword: "github actions",
    directory: "docs"
);

// 記事一覧を取得する
var articleList = await tools.ListArticles("docs");
var blogList = await tools.ListArticles("blog");
```

### MCPツールとしての使用

**ReadArticle**
```
WHEN TO USE:
- ateliers.devの技術記事を読み取る時
- Frontmatterメタデータが不要な時
- Markdown本文のみが必要な時

EXAMPLES:
✓ 'Read docs/csharp/datetime-extensions.md article'
✓ 'Show me blog/2024-11-26-mcp-server-development.md'
```

**ListArticles**
```
WHEN TO USE:
- 利用可能な記事を探索する時
- docsやblog内の記事構造を確認する時

EXAMPLES:
✓ 'List all articles in docs directory'
✓ 'Show all blog posts'
```

**SearchArticles**
```
WHEN TO USE:
- 特定のトピックに関する記事を探す時
- キーワードベースでコンテンツを発見する時

EXAMPLES:
✓ 'Search for github actions articles'
✓ 'Find articles about C# datetime'
```

## 🏗️ アーキテクチャ

### 依存関係

```
Ateliers.Ai.Mcp.Tools.Docusaurus (基盤)
            ↓
Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev (このパッケージ)
            ↓
    AteliersDevTools (実装)
```

### コンポーネント

- **AteliersDevTools** - ateliers.dev専用の記事管理ツール
- **GitHubService** - GitHubリポジトリとの連携
- **Frontmatter除去機能** - 正規表現によるYAMLメタデータの自動除去

## 🔗 関連パッケージ

### 基盤パッケージ

- **[Ateliers.Ai.Mcp.Tools.Docusaurus](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus/)** - Docusaurus基盤（MVP段階）
- **[Ateliers.Ai.Mcp.Tools](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/)** - MCPツール基盤
- **[Ateliers.Ai.Mcp.Services](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Services/)** - サービス層

### エコシステム

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
│  Ateliers.Ai.Mcp.Tools                  │  ツール基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  Ateliers.Ai.Mcp.Tools.Docusaurus       │  Docusaurus基盤
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│  このパッケージ                          │  ateliers.dev実装
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
