using Ateliers.Ai.Mcp.Services;
using Ateliers.Ai.Mcp.Services.GenericModels;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Ateliers.Ai.Mcp.Tools.Presentation;

[McpServerToolType]
public class PresentationVideoTool : McpToolBase
{
    private readonly IPresentationVideoGenerator _presentationVideoGenerator;

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

        IMPORTANT NARRATION RULES:
        - ナレーション文は **日本語（ひらがな・カタカナ・漢字）で記述してください**
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
        スライドのMarkdown内は **英語・アルファベット表記を使用できます**
        音声生成には影響しません

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
        try
        {
            var request = new PresentationVideoRequest
            {
                SourceMarkdown = sourceMarkdown,
                NarrationTexts = narrationTexts
            };

            var result = await _presentationVideoGenerator.GenerateAsync(request);

            return $"""
                SUCCESS: Presentation video generated successfully.

                VideoPath:
                {result.VideoPath}
                """;
        }
        catch (Exception ex)
        {
            // 想定外エラー
            return $"""
                ERROR:
                Type: {ex.GetType().Name}
                Message: {ex.Message}
                StackTrace:
                {ex.StackTrace}
                """;
        }
    }
}
