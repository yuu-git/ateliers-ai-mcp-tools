using Ateliers.Ai.Mcp.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ateliers.Ai.Mcp.Tools.McpLogAnalyzer;

/// <summary>
/// MCP ログ解析ツール
/// </summary>
/// <remarks>
/// McpLogAnalyzer Tool は MCP が出力したログを取得するための Tool です。
/// 解析・判断・要約は行わず、ログをそのまま返却します。
/// </remarks>
[McpServerToolType]
public sealed class McpLogAnalyzerTool : McpToolBase
{
    private readonly IMcpLogReader _logReader;

    /// <summary>
    /// ツール名
    /// </summary>
    protected override string ToolName => nameof(McpLogAnalyzerTool);

    public McpLogAnalyzerTool(IMcpLogger mcpLogger, IMcpLogReader logReader)
        : base(mcpLogger)
    {
        McpLogger?.Info($"{nameof(McpLogAnalyzerTool)} を初期化します。");

        if (logReader == null)
        {
            var ex = new ArgumentNullException(nameof(logReader));
            McpLogger?.Critical($"{nameof(McpLogAnalyzerTool)} の初期化に失敗しました。{nameof(IMcpLogReader)} は必須です。", ex);

            throw ex;
        }

        McpLogger?.Info($"{nameof(McpLogAnalyzerTool)} の初期化に成功しました。");

        _logReader = logReader;
    }

    /// <summary>
    /// 指定された相関IDに紐づくログを取得します。
    /// </summary>
    /// <param name="correlationId"> 相関ID </param>
    /// <returns> ログセッション </returns>
    [McpServerTool]
    [Description("""
        指定された相関IDに紐づくMCPログを取得します。
        
        WHEN TO USE:
        - MCPツールの実行エラーやログを調査したい時
        - 特定のツール実行の詳細な実行記録を確認したい時
        - エラーメッセージだけでは不十分で、内部動作を見たい時
        - デバッグやトラブルシューティングを行いたい時
        
        DO NOT USE WHEN:
        - 最新のログを見たい（use get_last_logs）
        - アプリケーションログではなく、MCPツール以外のログを見たい時
        
        LOG STRUCTURE:
        - ログは相関ID（CorrelationId）単位で記録されます
        - 各ツール実行時に一意の相関IDが付与されます
        - ログにはタイムスタンプ、ログレベル、カテゴリ、メッセージが含まれます
        
        CORRELATION ID:
        - 相関IDは通常、ツールのエラーメッセージやレスポンスに含まれます
        - 形式は GUID（例: 123e4567-e89b-12d3-a456-426614174000）です
        
        REQUIREMENTS:
        - MCPロガーが有効で、ログファイルが出力されていること
        - 指定した相関IDのログが存在すること
        
        EXAMPLES:
        ✓ 'CorrelationId: abc123 のログを取得して'
        ✓ 'このエラーの詳細ログを見せて（相関ID: xyz789）'
        ✓ 'プレゼンテーション動画生成が失敗したログを見たい'
        """)]
    public McpLogSession GetLogs(
        [Description("取得したいログの相関ID（必須）")]
        string correlationId)
    {
        using var scope = BeginToolExecution();
        
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            var ex = new ArgumentException("CorrelationId は空白にできません。", nameof(correlationId));
            McpLogger?.Error("ログの取得に失敗しました。CorrelationId が空白です。", ex);
            throw ex;
        }

        McpLogger?.Info($"MCP ログ解析ツールがログを取得します。CorrelationId: {correlationId}");

        return _logReader.ReadByCorrelationId(correlationId);
    }

    /// <summary>
    /// 最新のログを取得します。
    /// </summary>
    /// <returns> ログセッション </returns>
    [McpServerTool]
    [Description("""
        最新のMCPログセッションを取得します。
        
        WHEN TO USE:
        - 直前に実行したMCPツールのログを確認したい時
        - 相関IDが分からないが、最新の実行ログを見たい時
        - 'さっきの実行がなぜ失敗したのか' を調査したい時
        - 継続的にログをモニタリングしたい時
        
        DO NOT USE WHEN:
        - 特定の相関IDのログを見たい（use get_logs）
        - 過去の特定実行のログを取得したい時
        
        LOG STRUCTURE:
        - 最新のログセッション（最後に完了したツール実行）を返します
        - ログにはタイムスタンプ、ログレベル、カテゴリ、メッセージ、相関IDが含まれます
        - 複数のログエントリが時系列順に並びます
        
        TYPICAL USE CASES:
        - ツール実行直後のエラー調査
        - 'さっきのエラーの詳細を見せて'
        - 'なぜ失敗したのか教えて'
        - '前回の実行ログを見たい'
        
        REQUIREMENTS:
        - MCPロガーが有効で、ログファイルが出力されていること
        - 少なくとも1つのログセッションが存在すること
        
        EXAMPLES:
        ✓ '最新のログを見せて'
        ✓ 'さっきの実行が失敗した理由を調べて'
        ✓ '直前のプレゼンテーション動画生成のログを確認して'
        """)]
    public McpLogSession GetLastLogs()
    {
        McpLogger?.Info($"MCP ログ解析ツールが最新のログを取得します。");
        return _logReader.ReadLastSession();
    }
}

