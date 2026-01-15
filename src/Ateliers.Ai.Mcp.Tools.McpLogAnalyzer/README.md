# Ateliers.Ai.Mcp.Tools.McpLogAnalyzer

MCPツールの実行ログを取得・解析するためのModel Context Protocol (MCP)ツールを提供するC#ライブラリです。

## 概要

Ateliers.Ai.Mcp.Tools.McpLogAnalyzer は、MCPツールが出力したログを取得し、LLMに返すことで、エラー調査やトラブルシューティングを支援するツールです。

ログの解析や判断、要約は行わず、**ログをそのまま返却**します。これにより、LLMが詳細なログ情報を基に問題を診断できます。

## 主な機能

### 1. GetLogs - 相関IDによるログ取得

指定された相関ID（CorrelationId）に紐づくログセッションを取得します。

**使用シーン：**
- MCPツールの実行エラーやログを調査したい時
- 特定のツール実行の詳細な実行記録を確認したい時
- エラーメッセージだけでは不十分で、内部動作を見たい時
- デバッグやトラブルシューティングを行いたい時

### 2. GetLastLogs - 最新ログ取得

最新のMCPログセッションを取得します。

**使用シーン：**
- 直前に実行したMCPツールのログを確認したい時
- 相関IDが分からないが、最新の実行ログを見たい時
- さっきの実行がなぜ失敗したのか を調査したい時
- 継続的にログをモニタリングしたい時

## インストール

### NuGetパッケージ

```bash
dotnet add package Ateliers.Ai.Mcp.Tools.McpLogAnalyzer
```

### パッケージマネージャーコンソール

```powershell
Install-Package Ateliers.Ai.Mcp.Tools.McpLogAnalyzer
```

## ライセンス

MIT License

## リポジトリ

https://github.com/yuu-git/ateliers-ai-mcp-tools
