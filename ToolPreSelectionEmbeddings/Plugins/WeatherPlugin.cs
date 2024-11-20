using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class WeatherPlugin
{
    [KernelFunction, Description("Gets weather forecast for a location")]
    public async Task<string> GetForecast(
        [Description("The location to get weather for")] string location,
        [Description("Optional: number of days to forecast")] int days = 1)
    {
        // In a real implementation, you would integrate with a weather API
        // This is a mock implementation
        await Task.Delay(100); // Simulate API call
        return $"Mock weather forecast for {location} for the next {days} days: Sunny with a chance of clouds";
    }
} 