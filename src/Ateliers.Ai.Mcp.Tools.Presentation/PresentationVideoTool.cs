using Ateliers.Ai.Mcp.Services;
using Ateliers.Ai.Mcp.Services.GenericModels;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Ateliers.Ai.Mcp.Tools.Presentation;

[McpServerToolType]
public class PresentationVideoTool : McpToolBase
{
    private readonly IPresentationVideoGenerator _presentationVideoGenerator;

    /// <summary>
    /// ツール名
    /// </summary>
    protected override string ToolName => nameof(PresentationVideoTool);

    public PresentationVideoTool(
        IMcpLogger mcpLogger,
        IPresentationVideoGenerator presentationVideoGenerator)
        : base(mcpLogger)
    {
        _presentationVideoGenerator = presentationVideoGenerator;
    }

    [McpServerTool]
    [Description("""
        Markdown とナレーションからプレゼンテーション動画を生成します。
        
        WHEN TO USE:
        - Markdown からスライド＋音声付き動画を作りたい時
        - 社内資料・説明動画・納品用動画を生成したい時
        - Marp + VOICEVOX + FFmpeg をまとめて使いたい時
        
        DO NOT USE WHEN:
        - スライドだけ作りたい（use generate_slide_markdown）
        - 音声だけ作りたい（use generate_voice）

        GENERATION RULES:
        - スライドの数とナレーションの数は同じにしてください
        - マークダウンの見出し（# や ## など）で、スライドが分割されます
        - 現在は 見出し1（#） と 見出し2（##） のみがスライド区切りとして認識されます
        - 見出し3（###）以下は、スライド内のコンテンツとして扱われます

        IMPORTANT NARRATION RULES:
        - ナレーション文は **日本語（ひらがな・カタカナ・漢字）で記述してください**
        - ナレーション音声が正しく生成されないケースがあるため、漢字も常用外は避けて下さい
        - 英語・アルファベット表記は音声品質が大きく低下します

          EXAMPLES:
          - MCP -> エムシーピー
          - VS Code -> ブイエスコード
          - GitHub -> ギットハブ
          - AI Tool -> エーアイツール

        - 「・」「/」「|」「-」などの記号は、音声生成時に不自然な区切りになります
        - スペースも区切りとして解釈されるため、必要最小限にしてください

          EXAMPLES:
          - AI ・ MCP ・ Tool -> エーアイエムシーピーツール
          - プレゼン  動画 -> プレゼン動画

        MARKDOWN CONTENT RULES:
        - スライドのMarkdown内は **英語・アルファベット表記を使用できます**
        - 音声生成には影響しません

          OK EXAMPLES (Markdown):
          - # MCP とは
          - VS Code での開発
          - GitHub 連携

        REQUIREMENTS:
        - VOICEVOX がインストールされていること
        - Marp CLI がインストールされていること
        - FFmpeg がインストールされていること
        
        EXAMPLES:
        ✓ 'このMarkdownを動画にして'
        ✓ '各スライドにナレーションを付けて動画化して'
        """)]
    public async Task<string> GeneratePresentationVideo(
        [Description("プレゼンテーション用 Markdown（必須）")]
        string sourceMarkdown,

        [Description("各スライドに対応するナレーション文配列（必須）")]
        string[] narrationTexts)
    {
        var correlationId = Guid.NewGuid().ToString();
        var startedAt = DateTime.UtcNow;

        try
        {
            BeginToolExecution();

            McpLogger?.Info($"[{correlationId}] プレゼンテーション動画生成開始");

            var request = new PresentationVideoRequest
            {
                SourceMarkdown = sourceMarkdown,
                NarrationTexts = narrationTexts
            };

            var result = await _presentationVideoGenerator.GenerateAsync(request);

            var completedAt = DateTime.UtcNow;
            var duration = (completedAt - startedAt).TotalSeconds;

            McpLogger?.Info($"[{correlationId}] プレゼンテーション動画生成完了: {result.VideoPath}");

            var response = new PresentationVideoToolResponse
            {
                Success = true,
                CorrelationId = correlationId,
                VideoPath = result.VideoPath,
                StartedAt = startedAt,
                CompletedAt = completedAt,
                DurationSeconds = duration
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        catch (Exception ex)
        {
            var completedAt = DateTime.UtcNow;
            var duration = (completedAt - startedAt).TotalSeconds;

            McpLogger?.Error($"[{correlationId}] プレゼンテーション動画生成失敗: {ex.GetType().Name} - {ex.Message}", ex);

            var response = new PresentationVideoToolResponse
            {
                Success = false,
                CorrelationId = correlationId,
                ErrorType = ex.GetType().Name,
                ErrorMessage = ex.Message,
                ErrorStackTrace = ex.StackTrace,
                StartedAt = startedAt,
                CompletedAt = completedAt,
                DurationSeconds = duration
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
