using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Ateliers.Ai.Mcp.Services;

namespace Ateliers.Ai.Mcp.Tools.Docusaurus;

/// <summary>
/// Ateliers.dev 技術記事参照ツール
/// </summary>
[McpServerToolType]
public class AteliersDevTools : McpToolBase
{
    private readonly IGitHubService _gitHubService;

    /// <summary>
    /// ツール名
    /// </summary>
    protected override string ToolName => nameof(AteliersDevTools);

    public AteliersDevTools(IMcpLogger mcpLogger, IGitHubService gitHubService)
        : base(mcpLogger)
    {
        _gitHubService = gitHubService;
    }

    [McpServerTool]
    [Description(@"Read a technical article from ateliers.dev documentation. Automatically removes frontmatter.
        WHEN TO USE:
        - Reading technical articles from ateliers.dev
        - Accessing blog posts with frontmatter metadata
        - Need clean markdown content without YAML metadata
        - Referencing technical documentation

        DO NOT USE WHEN:
        - For non-article files (use read_repository_file instead)
        - Need to preserve frontmatter metadata
        - Reading from repositories other than AteliersDev

        EXAMPLES:
        ✓ 'Read docs/csharp/datetime-extensions.md article'
        ✓ 'Show me blog/2024-11-26-mcp-server-development.md'
        ✓ 'Get docs/github-guidelines/branch-strategy.md'

        RELATED TOOLS:
        - list_articles: Find articles before reading
        - search_articles: Find articles by keyword
        - read_repository_file: Read files with frontmatter preserved")]
    public async Task<string> ReadArticle(
        [Description("Relative path to the article file (e.g., 'docs/csharp/datetime-extensions.md')")]
        string filePath)
    {
        try
        {
            var content = await _gitHubService.GetFileContentAsync("AteliersDev", filePath);

            // Frontmatterを除去してMarkdown本文のみを返す
            var markdown = RemoveFrontmatter(content);

            return markdown;
        }
        catch (FileNotFoundException ex)
        {
            return $"Article not found: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Error reading article: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"List all technical articles in ateliers.dev documentation.
        WHEN TO USE:
        - Exploring available articles in docs or blog
        - Finding all markdown/mdx files
        - Discovering article structure
        - Browsing technical content

        DO NOT USE WHEN:
        - Need to search by keyword (use search_articles instead)
        - Working with non-article repositories
        - Already know exact file path (use read_article directly)

        EXAMPLES:
        ✓ 'List all articles in docs directory'
        ✓ 'Show all blog posts'
        ✓ 'List docs/github-guidelines articles'

        RELATED TOOLS:
        - search_articles: Find articles by keyword
        - read_article: Read specific article
        - list_repository_files: For non-article file listing")]
    public async Task<string> ListArticles(
        [Description("Directory to search (default: 'docs', can also be 'blog')")]
        string directory = "docs")
    {
        try
        {
            var files = await _gitHubService.ListFilesAsync(
                "AteliersDev",
                directory: directory,
                extension: ".md"
            );

            // .mdxファイルも追加で取得
            var mdxFiles = await _gitHubService.ListFilesAsync(
                "AteliersDev",
                directory: directory,
                extension: ".mdx"
            );

            var allFiles = files.Concat(mdxFiles).OrderBy(f => f).ToList();

            if (allFiles.Count == 0)
            {
                return $"No articles found in '{directory}' directory.";
            }

            return string.Join("\n", allFiles);
        }
        catch (Exception ex)
        {
            return $"Error listing articles: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Search technical articles by keyword in ateliers.dev documentation.
        WHEN TO USE:
        - Finding articles about specific topics
        - Searching for code examples or techniques
        - Discovering related content
        - Keyword-based content discovery

        DO NOT USE WHEN:
        - Already know exact article path (use read_article directly)
        - Need full text search across all files (use external search)
        - Searching in non-article repositories

        EXAMPLES:
        ✓ 'Search for github actions articles'
        ✓ 'Find articles about C# datetime'
        ✓ 'Search docs for async programming'

        RELATED TOOLS:
        - list_articles: Browse all available articles
        - read_article: Read articles found in search results")]
    public async Task<string> SearchArticles(
        [Description("Keyword to search for in article titles and content")]
        string keyword,
        [Description("Directory to search (default: 'docs', can also be 'blog')")]
        string directory = "docs")
    {
        try
        {
            var files = await _gitHubService.ListFilesAsync(
                "AteliersDev",
                directory: directory,
                extension: ".md"
            );

            var mdxFiles = await _gitHubService.ListFilesAsync(
                "AteliersDev",
                directory: directory,
                extension: ".mdx"
            );

            var allFiles = files.Concat(mdxFiles).ToList();
            var results = new List<string>();

            foreach (var file in allFiles)
            {
                // ファイル名にキーワードが含まれているか
                if (file.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add($"📄 {file} (matched in filename)");
                    continue;
                }

                // ファイル内容を検索
                try
                {
                    var content = await _gitHubService.GetFileContentAsync("AteliersDev", file);
                    var markdown = RemoveFrontmatter(content);

                    if (markdown.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        // マッチした行を抽出（最大3行）
                        var matchedLines = GetMatchedLines(markdown, keyword, maxLines: 3);
                        results.Add($"📄 {file}\n{matchedLines}");
                    }
                }
                catch
                {
                    // ファイル読み取りエラーは無視
                }
            }

            if (results.Count == 0)
            {
                return $"No articles found matching keyword '{keyword}' in '{directory}' directory.";
            }

            return string.Join("\n\n", results);
        }
        catch (Exception ex)
        {
            return $"Error searching articles: {ex.Message}";
        }
    }

    /// <summary>
    /// Frontmatter（---で囲まれたメタデータ）を除去
    /// </summary>
    private string RemoveFrontmatter(string content)
    {
        // Frontmatterのパターン: 先頭の"---"から次の"---"まで
        var pattern = @"^---\s*\n.*?\n---\s*\n";
        var result = Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        return result.Trim();
    }

    /// <summary>
    /// キーワードにマッチした行を抽出
    /// </summary>
    private string GetMatchedLines(string content, string keyword, int maxLines)
    {
        var lines = content.Split('\n');
        var matchedLines = lines
            .Where(line => line.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Take(maxLines)
            .Select(line => $"  > {line.Trim()}");

        return string.Join("\n", matchedLines);
    }
}