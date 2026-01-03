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
}
