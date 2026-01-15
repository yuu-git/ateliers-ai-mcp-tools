using ModelContextProtocol.Server;
using System.ComponentModel;
using Ateliers.Ai.Mcp.Services;
using Microsoft.Extensions.Options;
using Ateliers.Ai.Mcp;
using Ateliers.Ai.Mcp.Tools;

namespace Ateliers.Ai.McpServer.Tools;

/// <summary>
/// 汎用リポジトリ操作ツール
/// NOTE: AutoPull/AutoPush機能は Phase 6以降で再検討予定
/// </summary>
[McpServerToolType]
public class RepositoryTools : McpToolBase
{
    private readonly IGitHubService _gitHubService;
    private readonly IGitService _gitService;
    private readonly ILocalFileService _localFileService;

    /// <summary>
    /// ツール名
    /// </summary>
    protected override string ToolName => nameof(RepositoryTools);

    public RepositoryTools(
        IMcpLogger mcpLogger,
        IGitHubService gitHubService,
        ILocalFileService localFileService,
        IGitService gitService)
        : base(mcpLogger)
    {
        _gitHubService = gitHubService;
        _gitService = gitService;
        _localFileService = localFileService;
    }

    [McpServerTool]
    [Description(@"Read a file from any configured repository (local or GitHub).
        WHEN TO USE:
        - Need to read source code files
        - Need to read documentation or markdown files
        - Need to inspect configuration files
        - Need to analyze existing code before making changes
        DO NOT USE WHEN:
        - For article-specific operations (use read_article from AteliersDevTools)
        - File doesn't exist yet (use add_repository_file to create)
        AVAILABLE REPOSITORIES:
        - AteliersAiAssistants: Coding guidelines and training samples
        - AteliersAiMcpServer: MCP server source code
        - AteliersDev: Technical articles and blog posts
        - PublicNotes: TODO, ideas, and snippets
        - TrainingMcpServer: Training MCP server code
        EXAMPLES:
        ✓ 'Read Services/GitHubService.cs from AteliersAiMcpServer'
        ✓ 'Show me README.md from AteliersAiAssistants'
        ✓ 'Get appsettings.json from AteliersAiMcpServer'
        RELATED TOOLS:
        - list_repository_files: Find files before reading
        - edit_repository_file: Modify after reading
        - read_article: For ateliers.dev articles with frontmatter removal")]
    public async Task<string> ReadRepositoryFile(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("File path (e.g., 'README.md', 'Services/GitHubService.cs')")]
        string filePath)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            // ローカル優先
            if (!string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                var content = await _localFileService.ReadFileAsync(repositorySummary.LocalPath, filePath);
                return content;
            }

            // GitHubフォールバック
            return await _gitHubService.GetFileContentAsync(repositoryKey, filePath);
        }
        catch (FileNotFoundException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"List files in any configured repository (local or GitHub).
        WHEN TO USE:
        - Need to explore repository structure
        - Need to find specific files before reading
        - Need to see available files in a directory
        - Need to filter files by extension
        DO NOT USE WHEN:
        - For article discovery (use list_articles from AteliersDevTools)
        - When exact file path is already known
        AVAILABLE REPOSITORIES:
        - AteliersAiAssistants: Coding guidelines and training samples
        - AteliersAiMcpServer: MCP server source code
        - AteliersDev: Technical articles and blog posts
        - PublicNotes: TODO, ideas, and snippets
        - TrainingMcpServer: Training MCP server code
        EXAMPLES:
        ✓ 'List all markdown files in AteliersAiAssistants'
        ✓ 'Show C# files in AteliersAiMcpServer Services directory'
        ✓ 'List all files in AteliersDev docs directory'
        RELATED TOOLS:
        - read_repository_file: Read files after listing
        - search_articles: For keyword-based article search")]
    public async Task<string> ListRepositoryFiles(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("Directory path (empty for root, e.g., 'Services', 'docs')")]
        string directory = "",
        [Description("File extension filter (e.g., '.md', '.cs', leave empty for all)")]
        string? extension = null)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            List<string> files;

            // ローカル優先
            if (!string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                files = await _localFileService.ListFilesAsync(repositorySummary.LocalPath, directory, extension);
            }
            else
            {
                // GitHubフォールバック
                files = await _gitHubService.ListFilesAsync(repositoryKey, directory, extension);
            }

            if (files.Count == 0)
            {
                return $"📁 No files found in {repositoryKey}/{directory}";
            }

            var fileList = string.Join("\n", files.OrderBy(f => f));
            return $"📁 Files in {repositoryKey}/{directory} ({files.Count} files):\n\n{fileList}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Add a new file to a configured repository (local only).
        NOTE: Use GitTools for explicit Git operations (commit, push, pull).
        WHEN TO USE:
        - Creating new source code files
        - Creating new documentation files
        - Creating new configuration files
        - Generating new test files
        DO NOT USE WHEN:
        - File already exists (use edit_repository_file instead)
        - Repository LocalPath not configured
        EXAMPLES:
        ✓ 'Create test.txt in AteliersAiMcpServer with content Hello World'
        ✓ 'Add new guideline file guidelines/python/naming.md to AteliersAiAssistants'
        ✓ 'Create Services/NewService.cs in AteliersAiMcpServer'
        RELATED TOOLS:
        - edit_repository_file: Modify existing files
        - read_repository_file: Verify file contents after creation
        - list_repository_files: Check if file already exists
        - commit_and_push_repository: Commit and push changes (GitTools)")]
    public async Task<string> AddRepositoryFile(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("File path (e.g., 'test.txt', 'Services/NewService.cs')")]
        string filePath,
        [Description("File content")]
        string content)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            if (string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                return $"❌ LocalPath not configured for '{repositoryKey}'";
            }

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPull
            //if (repositorySummary.AutoPull)
            //{
            //    var pullResult = await _gitOperationService.PullAsync(repositoryKey, repositorySummary.LocalPath);
            //    if (!pullResult.Success)
            //        return $"❌ Pull failed: {pullResult.Message}";
            //}

            // ファイル作成
            await _localFileService.CreateFileAsync(repositorySummary.LocalPath, filePath, content);

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPush
            //if (repositorySummary.AutoPush)
            //{
            //    var pushResult = await _gitOperationService.CommitAndPushAsync(
            //        repositoryKey, repositorySummary.LocalPath, filePath);
            //    
            //    if (!pushResult.Success)
            //        return $"⚠️ Created but push failed: {pushResult.Message}";
            //    
            //    return $"✅ Created and pushed: {filePath} ({pushResult.CommitHash?[..7]})";
            //}

            return $"✅ Created: {filePath}";
        }
        catch (InvalidOperationException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Edit an existing file in a configured repository (local only). Automatically creates backup.
        NOTE: Use GitTools for explicit Git operations (commit, push, pull).
        WHEN TO USE:
        - Modifying existing source code
        - Updating documentation
        - Fixing bugs in existing files
        - Refactoring code
        DO NOT USE WHEN:
        - File doesn't exist yet (use add_repository_file instead)
        - Repository LocalPath not configured
        EXAMPLES:
        ✓ 'Update README.md in AteliersAiMcpServer with new content'
        ✓ 'Fix bug in Services/GitHubService.cs'
        ✓ 'Modify appsettings.json configuration'
        RELATED TOOLS:
        - read_repository_file: Read current contents before editing
        - backup_repository_file: Create additional backup if needed
        - add_repository_file: Create new files
        - commit_and_push_repository: Commit and push changes (GitTools)")]
    public async Task<string> EditRepositoryFile(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("File path (e.g., 'README.md', 'Services/GitHubService.cs')")]
        string filePath,
        [Description("New file content (replaces entire file)")]
        string content)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            if (string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                return $"❌ LocalPath not configured for '{repositoryKey}'";
            }

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPull
            //if (repositorySummary.AutoPull)
            //{
            //    var pullResult = await _gitOperationService.PullAsync(repositoryKey, repositorySummary.LocalPath);
            //    if (!pullResult.Success)
            //        return $"❌ Pull failed: {pullResult.Message}";
            //}

            // ファイル更新
            await _localFileService.UpdateFileAsync(repositorySummary.LocalPath, filePath, content, createBackup: true);

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPush
            //if (repositorySummary.AutoPush)
            //{
            //    var pushResult = await _gitOperationService.CommitAndPushAsync(
            //        repositoryKey, repositorySummary.LocalPath, filePath);
            //    
            //    if (!pushResult.Success)
            //        return $"⚠️ Updated but push failed: {pushResult.Message}";
            //    
            //    return $"✅ Updated and pushed: {filePath} ({pushResult.CommitHash?[..7]})";
            //}

            return $"✅ Updated: {filePath}";
        }
        catch (FileNotFoundException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Delete a file from a configured repository (local only). Automatically creates backup.
        NOTE: Use GitTools for explicit Git operations (commit, push, pull).
        WHEN TO USE:
        - Removing obsolete files
        - Cleaning up temporary files
        - Removing test files
        - Deleting backup files (.backup extension doesn't create backup)
        DO NOT USE WHEN:
        - Repository LocalPath not configured
        - File might be needed later (use rename_repository_file to archive instead)
        - Unsure about file importance (create manual backup first)
        EXAMPLES:
        ✓ 'Delete test.txt from AteliersAiMcpServer'
        ✓ 'Remove obsolete Service/OldService.cs'
        ✓ 'Clean up test.txt.backup file'
        RELATED TOOLS:
        - backup_repository_file: Create backup before deleting
        - rename_repository_file: Move to archive instead of deleting
        - list_repository_files: Verify file exists before deleting
        - commit_and_push_repository: Commit and push changes (GitTools)")]
            public async Task<string> DeleteRepositoryFile(
                [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("File path (e.g., 'test.txt', 'Services/OldService.cs')")]
        string filePath)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            if (string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                return $"❌ LocalPath not configured for '{repositoryKey}'";
            }

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPull
            //if (repositorySummary.AutoPush) // 削除後にプッシュするならプル必要
            //{
            //    var pullResult = await _gitOperationService.PullAsync(repositoryKey, repositorySummary.LocalPath);
            //    if (!pullResult.Success)
            //        return $"❌ Pull failed: {pullResult.Message}";
            //}

            // ファイル削除
            _localFileService.DeleteFile(repositorySummary.LocalPath, filePath, createBackup: true);
            
            var baseMessage = filePath.EndsWith(".backup")
                ? $"Deleted: {filePath}"
                : $"Deleted: {filePath} (backup created)";

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPush
            //if (repositorySummary.AutoPush)
            //{
            //    var pushResult = await _gitOperationService.CommitAndPushAsync(
            //        repositoryKey, repositorySummary.LocalPath, filePath);
            //    
            //    if (!pushResult.Success)
            //        return $"⚠️ {baseMessage} but push failed: {pushResult.Message}";
            //    
            //    return $"✅ {baseMessage} and pushed ({pushResult.CommitHash?[..7]})";
            //}

            return $"✅ {baseMessage}";
        }
        catch (FileNotFoundException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Rename a file in a configured repository (local only).
        NOTE: Use GitTools for explicit Git operations (commit, push, pull).
        WHEN TO USE:
        - Renaming files to follow naming conventions
        - Moving files to different directories
        - Archiving files without deleting
        - Reorganizing project structure
        DO NOT USE WHEN:
        - Repository LocalPath not configured
        - Destination file already exists
        - Want to keep both old and new (use copy_repository_file instead)
        EXAMPLES:
        ✓ 'Rename OldService.cs to NewService.cs in AteliersAiMcpServer'
        ✓ 'Move test.txt to archive/test.txt'
        ✓ 'Rename guideline.md to guidelines/csharp/naming.md'
        RELATED TOOLS:
        - copy_repository_file: Keep original file while creating new
        - delete_repository_file: Remove after renaming
        - list_repository_files: Verify new path doesn't exist
        - commit_and_push_repository: Commit and push changes (GitTools)")]
    public async Task<string> RenameRepositoryFile(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("Current file path (e.g., 'OldService.cs')")]
        string oldFilePath,
        [Description("New file path (e.g., 'Services/NewService.cs')")]
        string newFilePath)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            if (string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                return $"❌ LocalPath not configured for '{repositoryKey}'";
            }

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPull
            //if (repositorySummary.AutoPush)
            //{
            //    var pullResult = await _gitOperationService.PullAsync(repositoryKey, repositorySummary.LocalPath);
            //    if (!pullResult.Success)
            //        return $"❌ Pull failed: {pullResult.Message}";
            //}

            // ファイルリネーム
            _localFileService.RenameFile(repositorySummary.LocalPath, oldFilePath, newFilePath);

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPush
            //if (repositorySummary.AutoPush)
            //{
            //    // 旧ファイルと新ファイルの両方をステージング
            //    var commitResult = await _gitOperationService.CommitAsync(
            //        repositoryKey, repositorySummary.LocalPath, ".", // すべての変更をステージング
            //        $"Rename {oldFilePath} to {newFilePath} via MCP");
            //    
            //    if (!commitResult.Success)
            //        return $"⚠️ Renamed but commit failed: {commitResult.Message}";
            //
            //    var pushResult = await _gitOperationService.PushAsync(repositoryKey, repositorySummary.LocalPath);
            //    if (!pushResult.Success)
            //        return $"⚠️ Renamed and committed but push failed: {pushResult.Message}";
            //    
            //    return $"✅ Renamed and pushed: {oldFilePath} → {newFilePath} ({commitResult.CommitHash?[..7]})";
            //}

            return $"✅ Renamed: {oldFilePath} → {newFilePath}";
        }
        catch (FileNotFoundException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (InvalidOperationException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Copy a file within a configured repository (local only).
        NOTE: Use GitTools for explicit Git operations (commit, push, pull).
        WHEN TO USE:
        - Creating file templates or boilerplate
        - Duplicating configuration files for different environments
        - Creating backup before major modifications
        - Generating similar files with variations
        DO NOT USE WHEN:
        - Repository LocalPath not configured
        - Destination file exists and overwrite=false
        - Want to move file (use rename_repository_file instead)
        EXAMPLES:
        ✓ 'Copy template.md to new-guideline.md in AteliersAiAssistants'
        ✓ 'Copy appsettings.json to appsettings.Development.json'
        ✓ 'Duplicate Services/BaseService.cs to Services/NewService.cs'
        RELATED TOOLS:
        - rename_repository_file: Move instead of copy
        - backup_repository_file: Create timestamped backup
        - edit_repository_file: Modify after copying
        - commit_and_push_repository: Commit and push changes (GitTools)")]
    public async Task<string> CopyRepositoryFile(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("Source file path (e.g., 'template.md')")]
        string sourceFilePath,
        [Description("Destination file path (e.g., 'new-file.md')")]
        string destFilePath,
        [Description("Overwrite if destination exists (default: false)")]
        bool overwrite = false)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            if (string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                return $"❌ LocalPath not configured for '{repositoryKey}'";
            }

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPull
            //if (repositorySummary.AutoPush)
            //{
            //    var pullResult = await _gitOperationService.PullAsync(repositoryKey, repositorySummary.LocalPath);
            //    if (!pullResult.Success)
            //        return $"❌ Pull failed: {pullResult.Message}";
            //}

            // ファイルコピー
            _localFileService.CopyFile(repositorySummary.LocalPath, sourceFilePath, destFilePath, overwrite);

            // TODO: Phase 6 - AutoPull/AutoPush機能の再検討
            // AutoPush
            //if (repositorySummary.AutoPush)
            //{
            //    var pushResult = await _gitOperationService.CommitAndPushAsync(
            //        repositoryKey, repositorySummary.LocalPath, destFilePath,
            //        $"Copy {sourceFilePath} to {destFilePath} via MCP");
            //    
            //    if (!pushResult.Success)
            //        return $"⚠️ Copied but push failed: {pushResult.Message}";
            //    
            //    return $"✅ Copied and pushed: {sourceFilePath} → {destFilePath} ({pushResult.CommitHash?[..7]})";
            //}

            return $"✅ Copied: {sourceFilePath} → {destFilePath}";
        }
        catch (FileNotFoundException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (InvalidOperationException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description(@"Create a backup of a file in a configured repository (local only).
        WHEN TO USE:
        - Before making risky modifications
        - Creating timestamped snapshots
        - Preserving important file versions
        - Manual backup before automated operations
        DO NOT USE WHEN:
        - Repository LocalPath not configured
        - File doesn't exist
        - edit_repository_file already creates automatic backups
        EXAMPLES:
        ✓ 'Backup appsettings.json with suffix 2024-11-26'
        ✓ 'Create backup of Services/ImportantService.cs'
        ✓ 'Backup README.md before major rewrite'
        RELATED TOOLS:
        - edit_repository_file: Auto-creates .backup when editing
        - copy_repository_file: Create named copies
        - delete_repository_file: Auto-creates backup when deleting")]
    public async Task<string> BackupRepositoryFile(
        [Description("Repository key: AteliersAiAssistants, AteliersAiMcpServer, AteliersDev, PublicNotes, TrainingMcpServer")]
        string repositoryKey,
        [Description("File path (e.g., 'README.md', 'Services/GitHubService.cs')")]
        string filePath,
        [Description("Optional backup suffix (default: .backup)")]
        string? backupSuffix = null)
    {
        try
        {
            var repositorySummary = _gitHubService.GetRepositorySummary(repositoryKey);
            if (repositorySummary == null)
            {
                return $"❌ Repository '{repositoryKey}' not found";
            }

            if (string.IsNullOrEmpty(repositorySummary.LocalPath))
            {
                return $"❌ LocalPath not configured for '{repositoryKey}'";
            }

            _localFileService.BackupFile(repositorySummary.LocalPath, filePath, backupSuffix);
            var backupName = backupSuffix != null ? $"{filePath}.{backupSuffix}" : $"{filePath}.backup";
            return await Task.FromResult($"✅ Backup created: {backupName}");
        }
        catch (FileNotFoundException ex)
        {
            return $"❌ {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"❌ Error: {ex.Message}";
        }
    }
}