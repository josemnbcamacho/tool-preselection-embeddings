using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class UtilityPlugin
{
    [KernelFunction, Description("Extracts numbers from text")]
    public string ExtractNumbers(
        [Description("The text to extract numbers from")] string text,
        [Description("Optional: extract only integers")] bool integersOnly = false)
    {
        var numbers = System.Text.RegularExpressions.Regex.Matches(text, @"[-+]?\d*\.?\d+")
            .Select(m => m.Value)
            .Where(n => !integersOnly || !n.Contains("."));
        return string.Join(", ", numbers);
    }

    [KernelFunction, Description("Counts occurrences of a pattern in text")]
    public int CountOccurrences(
        [Description("The text to search in")] string text,
        [Description("The pattern to count")] string pattern,
        [Description("Optional: case sensitive search")] bool caseSensitive = false)
    {
        return caseSensitive 
                ? text.Split(new[] { pattern }, StringSplitOptions.None).Length - 1
                : Regex.Split(text, pattern, RegexOptions.IgnoreCase).Length - 1;
    }

    [KernelFunction, Description("Generates a unique identifier")]
    public string GenerateId(
        [Description("Optional: prefix for the ID")] string prefix = "",
        [Description("Optional: use timestamp in ID")] bool includeTimestamp = false)
    {
        var guid = Guid.NewGuid().ToString("N");
        return includeTimestamp
            ? $"{prefix}{DateTime.UtcNow:yyyyMMddHHmmss}-{guid}"
            : $"{prefix}{guid}";
    }

    [KernelFunction, Description("Validates if text matches a regex pattern")]
    public bool ValidatePattern(
        [Description("The text to validate")] string text,
        [Description("The regex pattern to match against")] string pattern)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(text, pattern);
    }

    [KernelFunction, Description("Generates random string")]
    public string GenerateRandomString(
        [Description("Length of the string")] int length,
        [Description("Character set to use (alpha, numeric, alphanumeric, all)")] string charSet = "alphanumeric")
    {
        var chars = charSet.ToLower() switch
        {
            "alpha" => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
            "numeric" => "0123456789",
            "alphanumeric" => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            "all" => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:,.<>?",
            _ => throw new ArgumentException("Invalid character set")
        };

        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [KernelFunction, Description("Checks if string is palindrome")]
    public bool IsPalindrome(
        [Description("The string to check")] string text,
        [Description("Optional: ignore case")] bool ignoreCase = true)
    {
        var processedText = ignoreCase ? text.ToLower() : text;
        return processedText.SequenceEqual(processedText.Reverse());
    }
} 