using Ateliers.Ai.Mcp.Logging;
using Ateliers.Ai.Mcp.Services;
using Ateliers.Ai.Mcp.Services.Ffmpeg;
using Ateliers.Ai.Mcp.Services.GenericModels;
using Ateliers.Ai.Mcp.Services.Marp;
using Ateliers.Ai.Mcp.Services.PresentationVideo;
using Ateliers.Ai.Mcp.Services.VoicePeak;
using System.Text.Json;

namespace Ateliers.Ai.Mcp.Tools.Presentation.IntegrationTests;

/// <summary>
/// PresentationVideoTool の統合テスト（VOICEPEAK版）
/// 実際のファイル生成を行うため、環境依存のテスト
/// ※ VoicePeak の CLI は処理が遅いため、テストは1つのみとする
/// </summary>
public sealed class PresentationVideoToolTests_VoicePeak
{
    // ★ 環境に合わせて書き換えてください
    private const string VoicePeakExecutablePath = @"C:\Program Files\VOICEPEAK\voicepeak.exe";
    private const string DefaultNarrator = "夏色花梨";
    private const string MarpExecutablePath = @"C:\Program Files\Marp-CLI\marp.exe";
    private const string FfmpegExecutablePath = @"C:\Program Files\FFmpeg\bin\ffmpeg.exe";

    [Fact(DisplayName = "VOICEPEAKを使用してプレゼンテーション動画が正しく生成されることを確認")]
    [Trait("Category", "Integration")]
    [Trait("Engine", "VoicePeak")]
    public async Task GeneratePresentationVideo_WithVoicePeak_VideoIsGenerated()
    {
        // Arrange
        var logger = new InMemoryMcpLogger(new McpLoggerOptions());

        // VoicePeak用オプション
        var voicePeakOptions = new VoicePeakServiceOptions
        {
            VoicePeakExecutablePath = VoicePeakExecutablePath,
            DefaultNarrator = DefaultNarrator,
            OutputRootDirectory = Path.Combine(Path.GetTempPath(), "presentations"),
            VoicePeakOutputDirectoryName = "voicepeak"
        };

        // Marp + FFmpeg + PresentationVideo用オプション
        var presentationOptions = new PresentationVideoServiceOptions
        {
            OutputRootDirectory = Path.Combine(Path.GetTempPath(), "presentations"),
            ResourcePath = string.Empty, // VoicePeakでは不要
            VoiceModelNames = null,
            VoicevoxOutputDirectoryName = "voice",
            MarpExecutablePath = MarpExecutablePath,
            MarpOutputDirectoryName = "marp",
            FfmpegExecutablePath = FfmpegExecutablePath,
            MediaOutputDirectoryName = "media"
        };

        var voicePeakService = new VoicePeakService(logger, voicePeakOptions);
        var marpService = new MarpService(logger, presentationOptions);
        var ffmpegService = new FfmpegService(logger, presentationOptions);
        var presentationVideoService = new PresentationVideoService(
            logger,
            presentationOptions,
            voicePeakService,
            marpService,
            ffmpegService);

        var tool = new PresentationVideoTool(logger, presentationVideoService);

        var markdown = """
            # テストプレゼンテーション
            これは最初のスライドです。
            
            ## ポイント1
            重要な情報がここにあります。
            
            # まとめ
            これで終わりです。
            """;
        var narrations = new[]
        {
            "テストプレゼンテーションへようこそ。これは最初のスライドです。",
            "ポイント1について説明します。重要な情報がここにあります。",
            "まとめです。これで終わりです。ご清聴ありがとうございました。"
        };

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.True(response.Success, $"動画生成が失敗しました: {response.ErrorMessage}");
        Assert.NotNull(response.CorrelationId);
        Assert.NotNull(response.VideoPath);

        // 動画ファイルが存在することを確認
        Assert.True(File.Exists(response.VideoPath), $"動画ファイルが存在しません: {response.VideoPath}");

        var fileInfo = new FileInfo(response.VideoPath);
        Assert.True(fileInfo.Length > 0, "動画ファイルサイズが0です");
    }
}
