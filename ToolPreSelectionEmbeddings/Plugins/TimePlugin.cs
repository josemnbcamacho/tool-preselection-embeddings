using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class TimePlugin
{
    [KernelFunction, Description("Gets the current time")]
    public string GetCurrentTime([Description("the specified timezone in C# compatible format, pass null here if no timezone is specified")] string? timezone = null)
    {
        if (string.IsNullOrEmpty(timezone))
        {
            return DateTime.Now.ToString("F");
        }
        
        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            return TimeZoneInfo.ConvertTime(DateTime.Now, tz).ToString("F");
        }
        catch (TimeZoneNotFoundException)
        {
            return $"Timezone '{timezone}' not found. Using local time: {DateTime.Now:F}";
        }
    }

    [KernelFunction, Description("Calculates time difference between two timestamps")]
    public string GetTimeDifference(
        [Description("First timestamp")] DateTime time1,
        [Description("Second timestamp")] DateTime time2,
        [Description("Optional: format (seconds, minutes, hours, days)")] string format = "minutes")
    {
        var diff = time2 - time1;
        return format.ToLower() switch
        {
            "seconds" => diff.TotalSeconds.ToString("F0"),
            "minutes" => diff.TotalMinutes.ToString("F0"),
            "hours" => diff.TotalHours.ToString("F1"),
            "days" => diff.TotalDays.ToString("F1"),
            _ => throw new ArgumentException("Invalid format. Use seconds, minutes, hours, or days.")
        };
    }

    [KernelFunction, Description("Checks if a given time is within business hours")]
    public bool IsBusinessHours(
        [Description("The time to check")] DateTime time,
        [Description("Optional: start hour (24h format)")] int startHour = 9,
        [Description("Optional: end hour (24h format)")] int endHour = 17)
    {
        return time.Hour >= startHour && time.Hour < endHour && time.DayOfWeek != DayOfWeek.Saturday && time.DayOfWeek != DayOfWeek.Sunday;
    }

    [KernelFunction, Description("Calculates next business day")]
    public DateTime GetNextBusinessDay(
        [Description("Starting date")] DateTime date)
    {
        do
        {
            date = date.AddDays(1);
        } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        
        return date;
    }

    [KernelFunction, Description("Calculates age from birthdate")]
    public int CalculateAge(
        [Description("Birthdate")] DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
} 