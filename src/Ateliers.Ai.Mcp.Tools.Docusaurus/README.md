# Ateliers.Ai.Mcp.Tools.Docusaurus

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Ateliers.Ai.Mcp.Tools.Docusaurus.svg)](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus/)

[Docusaurus](https://docusaurus.io/)ドキュメント管理のためのModel Context Protocol (MCP)ツールを提供するC#ライブラリです。

## 📦 インストール

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus
```

## 🎯 概要

`Ateliers.Ai.Mcp.Tools.Docusaurus`は、Docusaurusベースのドキュメントサイトと連携するための汎用的なMCPツールを提供します。記事の読み取り、検索、一覧表示など、ドキュメント管理に必要な機能を実装できます。

## ⚠️ 現在の状態

**MVP（Minimum Viable Product）段階**

このパッケージは現在、インターフェース定義と基盤クラスのみを提供しています。実際の機能実装は、具体的なドキュメントサイト向けの派生パッケージで提供されます：

- **[Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev/)** - ateliers.dev専用の実装

将来的には、以下の汎用機能を提供する予定です：

## 🔮 計画中の機能

### 記事管理
- 技術記事の読み取り（Frontmatterの自動除去）
- 記事の一覧表示（.md / .mdx対応）
- キーワードによる記事検索
- カテゴリー別の記事管理

### Docusaurus統合
- サイドバー構造の解析
- メタデータ（Frontmatter）の処理
- 多言語対応（i18n）
- バージョニングサポート

### コンテンツ操作
- マークダウンの解析と変換
- 記事のプレビュー生成
- リンク検証
- 画像参照の管理

## 📚 現在の使用方法

現時点では、このパッケージを直接使用するのではなく、具体的な実装パッケージを使用してください：

### Ateliers.dev向け実装

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev
```

```csharp
using Ateliers.Ai.Mcp.Tools.Docusaurus;

// ateliers.devの技術記事を読み取る
var tools = new AteliersDevTools(mcpLogger, gitHubService);
var article = await tools.ReadArticle("docs/csharp/datetime-extensions.md");

// 記事を検索する
var searchResults = await tools.SearchArticles("github actions", "docs");

// 記事一覧を取得する
var articleList = await tools.ListArticles("docs");
```

## 🏗️ アーキテクチャ

### 今後の構造（計画）

```
Ateliers.Ai.Mcp.Tools.Docusaurus (基盤)
├── IDocusaurusService (インターフェース)
├── DocusaurusToolBase (基底クラス)
├── FrontmatterParser (共通機能)
└── MarkdownProcessor (共通機能)
    ↓
具体的な実装パッケージ
├── Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev
└── その他のサイト向け実装...
```

## 🔗 関連パッケージ

### 依存関係

- [Ateliers.Ai.Mcp.Tools](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools/) - MCPツール基盤
- [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol/) - 公式MCP SDK

### 実装パッケージ

- **[Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev](https://www.nuget.org/packages/Ateliers.Ai.Mcp.Tools.Docusaurus.AteliersDev/)** - ateliers.dev専用ツール

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
│  実装パッケージ                          │  サイト固有実装
│  - AteliersDev                          │
└─────────────────────────────────────────┘
```

## 📖 ドキュメント

完全なドキュメント、使用例、ガイドについては **[ateliers.dev](https://ateliers.dev)** をご覧ください。

## 🚧 開発ステータス

このパッケージは開発中です（v0.x.x）。APIは予告なく変更される可能性があります。

**現在の状態**: MVP - 基盤クラスのみ提供  
**次のマイルストーン**: 汎用的なDocusaurus機能の実装

## 📄 ライセンス

このプロジェクトはMITライセンスの下でライセンスされています - 詳細は[LICENSE](https://github.com/yuu-git/ateliers-ai-mcp-tools/blob/master/LICENSE)ファイルをご覧ください。

## 🔗 リンク

- **Website**: [ateliers.dev](https://ateliers.dev)
- **GitHub**: [yuu-git/ateliers-ai-mcp-tools](https://github.com/yuu-git/ateliers-ai-mcp-tools)
- **NuGet**: [Ateliers Packages](https://www.nuget.org/profiles/ateliers)
- **Documentation**: [ateliers.dev](https://ateliers.dev)

---

Made with ❤️ by [ateliers.dev](https://ateliers.dev)
