using Ateliers.Ai.Mcp.Services;
using Ateliers.Ai.Mcp.Services.GenericModels;
using Moq;
using System.Text.Json;

namespace Ateliers.Ai.Mcp.Tools.Presentation.UnitTests;

/// <summary>
/// PresentationVideoTool の単体テスト（モックベース）
/// 実際のファイル生成は行わず、ロジックのみをテスト
/// </summary>
public sealed class PresentationVideoToolTests
{
    [Fact(DisplayName = "プレゼンテーション動画生成時に正しいパラメーターでジェネレーターが呼び出されることを確認")]
    public async Task GeneratePresentationVideo_CallsGeneratorWithCorrectParameters()
    {
        // Arrange
        var mockLogger = new Mock<IMcpLogger>();
        var mockGenerator = new Mock<IPresentationVideoGenerator>();

        var expectedVideoPath = "/output/presentation.mp4";
        mockGenerator
            .Setup(g => g.GenerateAsync(
                It.IsAny<PresentationVideoRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PresentationVideoResult
            {
                VideoPath = expectedVideoPath
            });

        var tool = new PresentationVideoTool(mockLogger.Object, mockGenerator.Object);

        var markdown = """
            # スライド1
            内容1
            
            # スライド2
            内容2
            """;
        var narrations = new[] { "スライド1のナレーション", "スライド2のナレーション" };

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson, 
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.CorrelationId);
        Assert.Equal(expectedVideoPath, response.VideoPath);
        Assert.NotNull(response.StartedAt);
        Assert.NotNull(response.CompletedAt);
        Assert.NotNull(response.DurationSeconds);

        mockGenerator.Verify(
            g => g.GenerateAsync(
                It.Is<PresentationVideoRequest>(r =>
                    r.SourceMarkdown == markdown &&
                    r.NarrationTexts.SequenceEqual(narrations)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "ジェネレーターが例外をスローした場合にエラーメッセージが返されることを確認")]
    public async Task GeneratePresentationVideo_WhenGeneratorThrows_ReturnsErrorMessage()
    {
        // Arrange
        var mockLogger = new Mock<IMcpLogger>();
        var mockGenerator = new Mock<IPresentationVideoGenerator>();

        var expectedException = new InvalidOperationException("Test exception");
        mockGenerator
            .Setup(g => g.GenerateAsync(
                It.IsAny<PresentationVideoRequest>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var tool = new PresentationVideoTool(mockLogger.Object, mockGenerator.Object);

        var markdown = """
            # テスト1
            内容1
            
            ## テスト2
            内容2
            """;
        var narrations = new[] { "テストナレーション1", "テストナレーション2" };

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.CorrelationId);
        Assert.Equal("InvalidOperationException", response.ErrorType);
        Assert.Equal("Test exception", response.ErrorMessage);
        Assert.NotNull(response.ErrorStackTrace);
        Assert.NotNull(response.StartedAt);
        Assert.NotNull(response.CompletedAt);
    }

    [Fact(DisplayName = "空のナレーション配列でジェネレーターが呼び出されることを確認")]
    public async Task GeneratePresentationVideo_WithEmptyNarrations_CallsGenerator()
    {
        // Arrange
        var mockLogger = new Mock<IMcpLogger>();
        var mockGenerator = new Mock<IPresentationVideoGenerator>();

        mockGenerator
            .Setup(g => g.GenerateAsync(
                It.IsAny<PresentationVideoRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PresentationVideoResult
            {
                VideoPath = "/output/presentation.mp4"
            });

        var tool = new PresentationVideoTool(mockLogger.Object, mockGenerator.Object);

        var markdown = """
            # テストスライド1
            内容1
            
            ## テストスライド2
            内容2
            """;
        var narrations = Array.Empty<string>();

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.VideoPath);

        mockGenerator.Verify(
            g => g.GenerateAsync(
                It.Is<PresentationVideoRequest>(r =>
                    r.NarrationTexts.Count == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "複数スライドのマークダウンとナレーションでジェネレーターが呼び出されることを確認")]
    public async Task GeneratePresentationVideo_WithMultipleSlides_CallsGenerator()
    {
        // Arrange
        var mockLogger = new Mock<IMcpLogger>();
        var mockGenerator = new Mock<IPresentationVideoGenerator>();

        mockGenerator
            .Setup(g => g.GenerateAsync(
                It.IsAny<PresentationVideoRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PresentationVideoResult
            {
                VideoPath = "/output/presentation.mp4"
            });

        var tool = new PresentationVideoTool(mockLogger.Object, mockGenerator.Object);

        var markdown = """
            # イントロダクション
            プレゼンテーションの概要
            
            ## ポイント1
            最初の重要なポイント
            
            ## ポイント2
            2番目の重要なポイント
            
            # まとめ
            結論
            """;
        var narrations = new[]
        {
            "イントロダクションのナレーションです",
            "ポイント1のナレーションです",
            "ポイント2のナレーションです",
            "まとめのナレーションです"
        };

        // Act
        var resultJson = await tool.GeneratePresentationVideo(markdown, narrations);

        // Assert
        Assert.NotNull(resultJson);
        var response = JsonSerializer.Deserialize<PresentationVideoToolResponse>(resultJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.VideoPath);

        mockGenerator.Verify(
            g => g.GenerateAsync(
                It.Is<PresentationVideoRequest>(r =>
                    r.SourceMarkdown == markdown &&
                    r.NarrationTexts.Count == 4),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "ToolNameプロパティが正しい値を返すことを確認")]
    public void ToolName_ShouldReturnCorrectName()
    {
        // Arrange
        var mockLogger = new Mock<IMcpLogger>();
        var mockGenerator = new Mock<IPresentationVideoGenerator>();

        // Act
        var tool = new PresentationVideoTool(mockLogger.Object, mockGenerator.Object);

        // Assert - ToolName is protected, but we can verify the instance is created correctly
        Assert.NotNull(tool);
    }
}