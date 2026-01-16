namespace Ateliers.Ai.Mcp.Tools.Presentation;

/// <summary>
/// PresentationVideoTool の実行結果
/// </summary>
public sealed record PresentationVideoToolResponse
{
    /// <summary>
    /// 処理の成功/失敗
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// 相関ID（ログ追跡用）
    /// </summary>
    public required string CorrelationId { get; init; }

    /// <summary>
    /// 生成された動画ファイルのパス（成功時のみ）
    /// </summary>
    public string? VideoPath { get; init; }

    /// <summary>
    /// エラーの種類（失敗時のみ）
    /// </summary>
    public string? ErrorType { get; init; }

    /// <summary>
    /// エラーメッセージ（失敗時のみ）
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// エラーのスタックトレース（失敗時のみ、デバッグ用）
    /// </summary>
    public string? ErrorStackTrace { get; init; }

    /// <summary>
    /// 処理開始日時
    /// </summary>
    public DateTime? StartedAt { get; init; }

    /// <summary>
    /// 処理完了日時
    /// </summary>
    public DateTime? CompletedAt { get; init; }

    /// <summary>
    /// 処理時間（秒）
    /// </summary>
    public double? DurationSeconds { get; init; }
}
