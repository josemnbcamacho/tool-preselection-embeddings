using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class StringManipulationPlugin
{
    [KernelFunction, Description("Reverses a string")]
    public string ReverseString(
        [Description("The string to reverse")] string input)
    {
        return new string(input.Reverse().ToArray());
    }

    [KernelFunction, Description("Removes specified characters from text")]
    public string RemoveCharacters(
        [Description("The input text")] string text,
        [Description("Characters to remove")] string chars)
    {
        return new string(text.Where(c => !chars.Contains(c)).ToArray());
    }

    [KernelFunction, Description("Finds the longest word in text")]
    public string FindLongestWord(
        [Description("The text to analyze")] string text)
    {
        return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .OrderByDescending(w => w.Length)
            .FirstOrDefault() ?? string.Empty;
    }
} 