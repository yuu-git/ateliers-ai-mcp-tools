using Ateliers.Ai.Mcp.Logging;

namespace Ateliers.Ai.Mcp.Tools.McpLogAnalyzer.UnitTests;

public class McpLogAnalyzerToolTests
{
    [Fact(DisplayName = "コンストラクタ: 有効なパラメータの場合、インスタンスが作成されること")]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        var loggerOption = new McpLoggerOptions();
        var logger = new FileMcpLogger(loggerOption);

        var tool = new McpLogAnalyzerTool(logger, logger);

        Assert.NotNull(tool);
    }

    [Fact(DisplayName = "コンストラクタ: logReaderがnullの場合、ArgumentNullExceptionがスローされること")]
    public void Constructor_WithNullLogReader_ShouldThrowArgumentNullException()
    {
        var loggerOption = new McpLoggerOptions();
        var logger = new FileMcpLogger(loggerOption);

        var exception = Assert.Throws<ArgumentNullException>(() => new McpLogAnalyzerTool(logger, null!));
        Assert.Equal("logReader", exception.ParamName);
    }

    [Fact(DisplayName = "GetLogs: 有効なCorrelationIdの場合、ログセッションが返されること")]
    public void GetLogs_WithValidCorrelationId_ShouldReturnLogSession()
    {
        var loggerOption = new McpLoggerOptions();
        var logger = new FileMcpLogger(loggerOption);
        var correlationId = Guid.NewGuid().ToString();

        logger.Info($"Test log entry for correlation ID: {correlationId}");

        var tool = new McpLogAnalyzerTool(logger, logger);
        var result = tool.GetLogs(correlationId);

        Assert.NotNull(result);
        Assert.NotNull(result.Entries);
    }

    [Fact(DisplayName = "GetLogs: 空白のCorrelationIdの場合、ArgumentExceptionがスローされること")]
    public void GetLogs_WithEmptyCorrelationId_ShouldThrowArgumentException()
    {
        var loggerOption = new McpLoggerOptions();
        var logger = new FileMcpLogger(loggerOption);
        var tool = new McpLogAnalyzerTool(logger, logger);

        var exception = Assert.Throws<ArgumentException>(() => tool.GetLogs(string.Empty));
        Assert.Equal("correlationId", exception.ParamName);
    }

    [Fact(DisplayName = "GetLogs: nullのCorrelationIdの場合、ArgumentExceptionがスローされること")]
    public void GetLogs_WithNullCorrelationId_ShouldThrowArgumentException()
    {
        var loggerOption = new McpLoggerOptions();
        var logger = new FileMcpLogger(loggerOption);
        var tool = new McpLogAnalyzerTool(logger, logger);

        var exception = Assert.Throws<ArgumentException>(() => tool.GetLogs(null!));
        Assert.Equal("correlationId", exception.ParamName);
    }

    [Fact(DisplayName = "GetLastLogs: 最新のログセッションが返されること")]
    public void GetLastLogs_ShouldReturnLastLogSession()
    {
        var loggerOption = new McpLoggerOptions();
        var logger = new FileMcpLogger(loggerOption);

        logger.Info("Test log entry for last session");

        var tool = new McpLogAnalyzerTool(logger, logger);
        var result = tool.GetLastLogs();

        Assert.NotNull(result);
        Assert.NotNull(result.Entries);
    }
}
