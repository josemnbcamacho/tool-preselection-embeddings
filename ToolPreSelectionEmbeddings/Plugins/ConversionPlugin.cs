using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class ConversionPlugin
{
    private static readonly Dictionary<string, double> LengthConversions = new()
    {
        {"m_to_ft", 3.28084},
        {"ft_to_m", 0.3048},
        {"km_to_mi", 0.621371},
        {"mi_to_km", 1.60934}
    };

    private static readonly Dictionary<string, double> WeightConversions = new()
    {
        {"kg_to_lb", 2.20462},
        {"lb_to_kg", 0.453592}
    };

    [KernelFunction, Description("Converts between different length units")]
    public double ConvertLength(
        [Description("The value to convert")] double value,
        [Description("The source unit (m, ft, km, mi)")] string fromUnit,
        [Description("The target unit (m, ft, km, mi)")] string toUnit)
    {
        var key = $"{fromUnit.ToLower()}_to_{toUnit.ToLower()}";
        if (LengthConversions.TryGetValue(key, out var factor))
        {
            return value * factor;
        }
        throw new ArgumentException($"Unsupported conversion: {fromUnit} to {toUnit}");
    }

    [KernelFunction, Description("Converts between different weight units")]
    public double ConvertWeight(
        [Description("The value to convert")] double value,
        [Description("The source unit (kg, lb)")] string fromUnit,
        [Description("The target unit (kg, lb)")] string toUnit)
    {
        var key = $"{fromUnit.ToLower()}_to_{toUnit.ToLower()}";
        if (WeightConversions.TryGetValue(key, out var factor))
        {
            return value * factor;
        }
        throw new ArgumentException($"Unsupported conversion: {fromUnit} to {toUnit}");
    }
} 