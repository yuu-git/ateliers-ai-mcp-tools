using Ateliers.Ai.Mcp.Logging;
using Ateliers.Ai.Mcp.Services;
using Ateliers.Ai.Mcp.Services.Ffmpeg;
using Ateliers.Ai.Mcp.Services.GenericModels;
using Ateliers.Ai.Mcp.Services.Marp;
using Ateliers.Ai.Mcp.Services.PresentationVideo;
using Ateliers.Ai.Mcp.Services.Voicevox;
using System.Text.Json;

namespace Ateliers.Ai.Mcp.Tools.Presentation.IntegrationTests;

/// <summary>
/// PresentationVideoTool の統合テスト（VOICEVOX版）
/// 実際のファイル生成を行うため、環境依存のテスト
/// </summary>
public sealed class PresentationVideoToolTests
{
    // ★ 環境に合わせて書き換えてください
    private const string VoicevoxResourcePath = @"C:\Program Files\VOICEVOX\vv-engine";
    private const string MarpExecutablePath = @"C:\Program Files\Marp-CLI\marp.exe";
    private const string FfmpegExecutablePath = @"C:\Program Files\FFmpeg\bin\ffmpeg.exe";

    public PresentationVideoToolTests()
    {
        NativeLibraryPath.Use(VoicevoxResourcePath);
    }

    [Fact(DisplayName = "VOICEVOXを使用してプレゼンテーション動画が正しく生成されることを確認")]
    [Trait("Category", "Integration")]
    [Trait("Engine", "Voicevox")]
    public async Task GeneratePresentationVideo_WithVoicevox_VideoIsGenerated()
    {
        // Arrange
        var logger = new InMemoryMcpLogger(new McpLoggerOptions());
        var options = new PresentationVideoServiceOptions
        {
            OutputRootDirectory = Path.Combine(Path.GetTempPath(), "presentations"),
            ResourcePath = VoicevoxResourcePath,
            VoiceModelNames = new[] { "0.vmm" },
            VoicevoxOutputDirectoryName = "voicevox",
            MarpExecutablePath = MarpExecutablePath,
            MarpOutputDirectoryName = "marp",
            FfmpegExecutablePath = FfmpegExecutablePath,
            MediaOutputDirectoryName = "media"
        };

        var voicevoxService = new VoicevoxService(logger, options);
        var marpService = new MarpService(logger, options);
        var ffmpegService = new FfmpegService(logger, options);
        var presentationVideoService = new PresentationVideoService(
            logger,
            options,
            voicevoxService,
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

        var a = logger.ReadLastSession();

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.CorrelationId);
        Assert.NotNull(response.VideoPath);
        
        // 動画ファイルが存在することを確認
        Assert.True(File.Exists(response.VideoPath), $"動画ファイルが存在しません: {response.VideoPath}");

        var fileInfo = new FileInfo(response.VideoPath);
        Assert.True(fileInfo.Length > 0, "動画ファイルサイズが0です");
    }

    [Fact(DisplayName = "VOICEVOXで単一スライドのプレゼンテーション動画が生成されることを確認")]
    [Trait("Category", "Integration")]
    [Trait("Engine", "Voicevox")]
    public async Task GeneratePresentationVideo_WithVoicevox_SingleSlide_VideoIsGenerated()
    {
        // Arrange
        var logger = new InMemoryMcpLogger(new McpLoggerOptions());
        var options = new PresentationVideoServiceOptions
        {
            OutputRootDirectory = Path.Combine(Path.GetTempPath(), "presentations"),
            ResourcePath = VoicevoxResourcePath,
            VoiceModelNames = new[] { "0.vmm" },
            VoicevoxOutputDirectoryName = "voicevox",
            MarpExecutablePath = MarpExecutablePath,
            MarpOutputDirectoryName = "marp",
            FfmpegExecutablePath = FfmpegExecutablePath,
            MediaOutputDirectoryName = "media"
        };

        var voicevoxService = new VoicevoxService(logger, options);
        var marpService = new MarpService(logger, options);
        var ffmpegService = new FfmpegService(logger, options);
        var presentationVideoService = new PresentationVideoService(
            logger,
            options,
            voicevoxService,
            marpService,
            ffmpegService);

        var tool = new PresentationVideoTool(logger, presentationVideoService);

        var markdown = """
            # スライド1
            これは最初のスライドです。
            
            ## スライド2
            これは2番目のスライドです。
            """;
        var narrations = new[]
        {
            "最初のスライドのプレゼンテーションです。",
            "2番目のスライドのプレゼンテーションです。"
        };

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.True(response.Success, $"動画生成が失敗しました: {response.ErrorMessage}");
        Assert.NotNull(response.VideoPath);
        Assert.True(File.Exists(response.VideoPath), $"動画ファイルが存在しません: {response.VideoPath}");
    }

    [Fact(DisplayName = "VOICEVOXで複雑なマークダウンのプレゼンテーション動画が生成されることを確認")]
    [Trait("Category", "Integration")]
    [Trait("Engine", "Voicevox")]
    public async Task GeneratePresentationVideo_WithVoicevox_ComplexMarkdown_VideoIsGenerated()
    {
        // Arrange
        var logger = new InMemoryMcpLogger(new McpLoggerOptions());
        var options = new PresentationVideoServiceOptions
        {
            OutputRootDirectory = Path.Combine(Path.GetTempPath(), "presentations"),
            ResourcePath = VoicevoxResourcePath,
            VoiceModelNames = new[] { "0.vmm" },
            VoicevoxOutputDirectoryName = "voicevox",
            MarpExecutablePath = MarpExecutablePath,
            MarpOutputDirectoryName = "marp",
            FfmpegExecutablePath = FfmpegExecutablePath,
            MediaOutputDirectoryName = "media"
        };

        var voicevoxService = new VoicevoxService(logger, options);
        var marpService = new MarpService(logger, options);
        var ffmpegService = new FfmpegService(logger, options);
        var presentationVideoService = new PresentationVideoService(
            logger,
            options,
            voicevoxService,
            marpService,
            ffmpegService);

        var tool = new PresentationVideoTool(logger, presentationVideoService);

        var markdown = """
            # イントロダクション
            
            これは複雑なマークダウンのテストです。
            
            - リスト項目1
            - リスト項目2
            - リスト項目3
            
            ## サブセクション
            
            詳細な説明がここにあります。
            
            ### さらに詳細
            
            より深い内容です。
            
            # 結論
            
            まとめです。
            """;
        var narrations = new[]
        {
            "イントロダクションです。これは複雑なマークダウンのテストです。",
            "詳細な説明がここにあります。より深い内容についても触れます。",
            "結論です。まとめです。"
        };

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.True(response.Success, $"動画生成が失敗しました: {response.ErrorMessage}");
        Assert.NotNull(response.VideoPath);
        Assert.True(File.Exists(response.VideoPath), $"動画ファイルが存在しません: {response.VideoPath}");
    }
}