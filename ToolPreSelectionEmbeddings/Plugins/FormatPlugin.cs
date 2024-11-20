using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class FormatPlugin
{
    [KernelFunction, Description("Formats JSON data")]
    public string FormatJson(
        [Description("The JSON string to format")] string json,
        [Description("Optional: indentation level (default 2)")] int indentation = 2)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var element = JsonSerializer.Deserialize<JsonElement>(json);
        return JsonSerializer.Serialize(element, options);
    }

    [KernelFunction, Description("Formats dates in various styles")]
    public string FormatDate(
        [Description("The date to format")] DateTime date,
        [Description("The format style (short, long, full)")] string style = "long")
    {
        return style.ToLower() switch
        {
            "short" => date.ToString("d"),
            "long" => date.ToString("D"),
            "full" => date.ToString("F"),
            _ => throw new ArgumentException("Invalid format style. Use 'short', 'long', or 'full'.")
        };
    }

    [KernelFunction, Description("Formats numbers with specified precision")]
    public string FormatNumber(
        [Description("The number to format")] double number,
        [Description("The number of decimal places")] int decimals = 2,
        [Description("Optional: use thousands separator")] bool useThousandsSeparator = true)
    {
        var format = useThousandsSeparator ? "N" : "F";
        return number.ToString($"{format}{decimals}");
    }
} 