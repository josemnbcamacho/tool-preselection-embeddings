using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class FilePlugin
{
    [KernelFunction, Description("Searches file contents across directories")]
    public async Task<string> SearchContent(
        [Description("The search pattern or text to find")] string searchPattern,
        [Description("The directory path to search in")] string directory,
        [Description("Optional: file extensions to search (comma-separated)")] string fileTypes = "*.*")
    {
        var extensions = fileTypes.Split(',').Select(x => x.Trim()).ToArray();
        var results = new List<string>();
        
        foreach (var ext in extensions)
        {
            var files = Directory.GetFiles(directory, ext, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                if (content.Contains(searchPattern, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add($"Found in {file}");
                }
            }
        }
        
        return results.Any() 
            ? string.Join("\n", results) 
            : "No matches found";
    }

    [KernelFunction, Description("Gets file metadata including size and dates")]
    public string GetFileInfo([Description("The path to the file")] string filePath)
    {
        var info = new FileInfo(filePath);
        return $"""
            File: {info.Name}
            Size: {info.Length:N0} bytes
            Created: {info.CreationTime}
            Modified: {info.LastWriteTime}
            """;
    }
} 