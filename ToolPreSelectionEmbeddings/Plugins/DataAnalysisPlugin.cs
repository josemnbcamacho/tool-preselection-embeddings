using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class DataAnalysisPlugin
{
    [KernelFunction, Description("Calculates basic statistics (min, max, avg) from a list of numbers")]
    public string CalculateStats(
        [Description("Comma-separated list of numbers")] string numbers)
    {
        var nums = numbers.Split(',')
            .Select(n => double.Parse(n.Trim()))
            .ToList();
        
        return $"Min: {nums.Min():F2}, Max: {nums.Max():F2}, Average: {nums.Average():F2}";
    }

    [KernelFunction, Description("Finds outliers in a dataset using IQR method")]
    public string FindOutliers(
        [Description("Comma-separated list of numbers")] string numbers)
    {
        var nums = numbers.Split(',')
            .Select(n => double.Parse(n.Trim()))
            .OrderBy(n => n)
            .ToList();
        
        var q1 = nums[nums.Count / 4];
        var q3 = nums[nums.Count * 3 / 4];
        var iqr = q3 - q1;
        var lowerBound = q1 - 1.5 * iqr;
        var upperBound = q3 + 1.5 * iqr;
        
        var outliers = nums.Where(n => n < lowerBound || n > upperBound);
        return string.Join(", ", outliers);
    }

    [KernelFunction, Description("Calculates percentage change between two values")]
    public double CalculatePercentageChange(
        [Description("Original value")] double oldValue,
        [Description("New value")] double newValue)
    {
        return ((newValue - oldValue) / oldValue) * 100;
    }
} 