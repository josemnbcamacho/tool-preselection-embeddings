using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class TextPlugin
{
    private readonly Kernel _kernel;

    public TextPlugin(Kernel kernel)
    {
        _kernel = kernel;
    }

    [KernelFunction, Description("Generates text summaries")]
    public async Task<string> Summarize(
        [Description("The text to summarize")] string text,
        [Description("Optional: desired length of summary in words")] int? maxWords = null)
    {
        var prompt = @"Summarize the following text while maintaining key points and context.
If maxWords is specified, keep the summary within that word limit.

Text to summarize: {{$text}}
Maximum words: {{$maxWords}}

Summary:";

        var arguments = new KernelArguments
        {
            ["text"] = text,
            ["maxWords"] = maxWords?.ToString() ?? "not specified"
        };

        var result = await _kernel.InvokePromptAsync(prompt, arguments);
        return result.GetValue<string>() ?? string.Empty;
    }

    [KernelFunction, Description("Extracts keywords from text")]
    public async Task<string> ExtractKeywords(
        [Description("The text to extract keywords from")] string text,
        [Description("Optional: maximum number of keywords")] int maxKeywords = 5)
    {
        var prompt = @"Extract the most important keywords from the following text, up to the specified maximum number.
        Text: {{$text}}
        Maximum keywords: {{$maxKeywords}}
        
        Keywords:";

        var arguments = new KernelArguments
        {
            ["text"] = text,
            ["maxKeywords"] = maxKeywords.ToString()
        };

        var result = await _kernel.InvokePromptAsync(prompt, arguments);
        return result.GetValue<string>() ?? string.Empty;
    }

    [KernelFunction, Description("Detects the language of text")]
    public async Task<string> DetectLanguage([Description("The text to analyze")] string text)
    {
        var prompt = @"Analyze the following text and determine its language. Return only the language name in English.
        Text: {{$text}}
        
        Language:";

        var arguments = new KernelArguments { ["text"] = text };
        var result = await _kernel.InvokePromptAsync(prompt, arguments);
        return result.GetValue<string>() ?? string.Empty;
    }

    [KernelFunction, Description("Finds common words between two texts")]
    public string FindCommonWords(
        [Description("First text")] string text1,
        [Description("Second text")] string text2)
    {
        var words1 = text1.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.ToLower())
            .ToHashSet();
        
        var words2 = text2.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.ToLower());
        
        return string.Join(", ", words1.Intersect(words2));
    }

    [KernelFunction, Description("Calculates text similarity score")]
    public double CalculateTextSimilarity(
        [Description("First text")] string text1,
        [Description("Second text")] string text2)
    {
        var words1 = text1.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.ToLower())
            .ToHashSet();
        
        var words2 = text2.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.ToLower())
            .ToHashSet();
        
        var intersection = words1.Intersect(words2).Count();
        var union = words1.Union(words2).Count();
        
        return (double)intersection / union;
    }
} 