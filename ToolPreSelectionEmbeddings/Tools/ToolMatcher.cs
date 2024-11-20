namespace ToolPreSelectionEmbeddings.Tools;

public static class ToolMatcher
{
    private const double MinimumScoreThreshold = 0.75;

    public static async Task<IEnumerable<(string Name, string PluginName, string Description)>> FindBestMatch(
        string hypotheticalDescription, 
        ToolRegistry registry)
    {
        var searchResults = await registry.SearchToolsAsync(hypotheticalDescription);
        
        return searchResults
            .Where(x => x.Score >= MinimumScoreThreshold)
            .Select(match => (
                match.Tool.Name,
                match.Tool.PluginName,
                match.Tool.Description));
    }
}