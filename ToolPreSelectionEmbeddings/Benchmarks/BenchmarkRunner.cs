using ToolPreSelectionEmbeddings.Tools;

namespace ToolPreSelectionEmbeddings.Benchmarks;

public static class BenchmarkRunner
{
    public static void PrintMatches(IEnumerable<(string Name, string PluginName, string Description)> matches)
    {
        if (!matches.Any())
        {
            Console.WriteLine("- No matches found");
            return;
        }
        
        foreach (var match in matches)
        {
            Console.WriteLine($"- {match.Name} ({match.PluginName}): {match.Description}");
        }
    }

    public static async Task RunBenchmark(ToolRegistry toolRegistry, 
        DescriptionGenerator descriptionGenerator)
    {
        Console.WriteLine("\n=== Running Benchmark ===\n");
        
        var hydeResults = new List<(string Query, string ExpectedTool, IEnumerable<(string Name, string PluginName, string Description)> Matches, TimeSpan Duration)>();
        var directResults = new List<(string Query, string ExpectedTool, IEnumerable<(string Name, string PluginName, string Description)> Matches, TimeSpan Duration)>();

        foreach (var (query, expectedTool) in BenchmarkData.Queries)
        {
            Console.WriteLine($"Testing query: {query}");
            
            // Test HyDE approach
            var hydeStart = DateTime.UtcNow;
            var hydeDescription = await descriptionGenerator.GenerateDescription(query);
            var hydeMatches = await ToolMatcher.FindBestMatch(hydeDescription, toolRegistry);
            var hydeDuration = DateTime.UtcNow - hydeStart;
            hydeResults.Add((query, expectedTool, hydeMatches, hydeDuration));

            // Test direct approach
            var directStart = DateTime.UtcNow;
            var directMatches = await ToolMatcher.FindBestMatch(query, toolRegistry);
            var directDuration = DateTime.UtcNow - directStart;
            directResults.Add((query, expectedTool, directMatches, directDuration));
        }

        // Print results
        PrintBenchmarkResults("HyDE Approach", hydeResults);
        PrintBenchmarkResults("Direct Approach", directResults);
        
        // Compare accuracy
        var hydeAccuracy = CalculateAccuracy(hydeResults);
        var directAccuracy = CalculateAccuracy(directResults);
        
        Console.WriteLine("\n=== Final Results ===");
        Console.WriteLine($"HyDE Approach Accuracy: {hydeAccuracy:P2}");
        Console.WriteLine($"Direct Approach Accuracy: {directAccuracy:P2}");
        Console.WriteLine($"Average HyDE Time: {hydeResults.Average(r => r.Duration.TotalMilliseconds):F2}ms");
        Console.WriteLine($"Average Direct Time: {directResults.Average(r => r.Duration.TotalMilliseconds):F2}ms");
    }

    private static void PrintBenchmarkResults(
        string approach, 
        List<(string Query, string ExpectedTool, IEnumerable<(string Name, string PluginName, string Description)> Matches, TimeSpan Duration)> results)
    {
        Console.WriteLine($"\n=== {approach} Results ===");
        foreach (var result in results)
        {
            Console.WriteLine($"\nQuery: {result.Query}");
            Console.WriteLine($"Expected: {result.ExpectedTool}");
            Console.WriteLine($"Time: {result.Duration.TotalMilliseconds:F2}ms");
            Console.WriteLine("Top matches:");
            foreach (var match in result.Matches.Take(3))
            {
                Console.WriteLine($"- {match.Name} ({match.PluginName})");
            }
        }
    }

    private static double CalculateAccuracy(
        List<(string Query, string ExpectedTool, IEnumerable<(string Name, string PluginName, string Description)> Matches, TimeSpan Duration)> results)
    {
        int correctMatches = results.Count(r => 
            r.Matches.Any(m => m.Name.Equals(r.ExpectedTool, StringComparison.OrdinalIgnoreCase)));
        return (double)correctMatches / results.Count;
    }
}