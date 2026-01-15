namespace Ateliers.Ai.Mcp.Tools;

/// <summary>
/// MCPツールの基底クラス
/// </summary>
public abstract class McpToolBase
{
    /// <summary>
    /// MCPロガー
    /// </summary>
    protected IMcpLogger? McpLogger { get; }

    /// <summary>
    /// ツール名（各派生クラスで実装）
    /// </summary>
    protected abstract string ToolName { get; }

    /// <summary>
    /// 現在の実行コンテキストの相関IDを取得します。
    /// </summary>
    protected string? CurrentCorrelationId => Ai.Mcp.Context.McpExecutionContext.Current?.CorrelationId;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public McpToolBase()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="mcpLogger"> MCPロガー </param>
    public McpToolBase(IMcpLogger mcpLogger)
        : this()
    {
        McpLogger = mcpLogger;
    }

    /// <summary>
    /// ツールメソッドの実行スコープを開始します。
    /// 各ツールメソッドの冒頭で使用することで、実行ごとに一意の相関IDが生成されます。
    /// </summary>
    /// <returns>実行スコープ（usingで使用）</returns>
    protected Ai.Mcp.Context.McpExecutionContextScope BeginToolExecution()
    {
        return new Ai.Mcp.Context.McpExecutionContextScope(ToolName);
    }
}
