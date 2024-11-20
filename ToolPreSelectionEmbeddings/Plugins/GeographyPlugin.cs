using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class GeographyPlugin
{
    private const double EarthRadiusKm = 6371.0;

    [KernelFunction, Description("Calculates distance between two coordinates")]
    public double CalculateDistance(
        [Description("Latitude of first point")] double lat1,
        [Description("Longitude of first point")] double lon1,
        [Description("Latitude of second point")] double lat2,
        [Description("Longitude of second point")] double lon2)
    {
        var dLat = ToRad(lat2 - lat1);
        var dLon = ToRad(lon2 - lon1);
        
        var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                Math.Sin(dLon/2) * Math.Sin(dLon/2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
        return EarthRadiusKm * c;
    }

    [KernelFunction, Description("Validates geographic coordinates")]
    public bool ValidateCoordinates(
        [Description("Latitude to validate")] double latitude,
        [Description("Longitude to validate")] double longitude)
    {
        return latitude >= -90 && latitude <= 90 &&
               longitude >= -180 && longitude <= 180;
    }

    [KernelFunction, Description("Converts coordinates to cardinal direction")]
    public string GetCardinalDirection(
        [Description("Starting latitude")] double lat1,
        [Description("Starting longitude")] double lon1,
        [Description("Destination latitude")] double lat2,
        [Description("Destination longitude")] double lon2)
    {
        var bearing = CalculateBearing(lat1, lon1, lat2, lon2);
        return bearing switch
        {
            < 22.5 => "N",
            < 67.5 => "NE",
            < 112.5 => "E",
            < 157.5 => "SE",
            < 202.5 => "S",
            < 247.5 => "SW",
            < 292.5 => "W",
            < 337.5 => "NW",
            _ => "N"
        };
    }

    private static double ToRad(double degrees) => degrees * Math.PI / 180.0;

    private double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
    {
        var dLon = ToRad(lon2 - lon1);
        lat1 = ToRad(lat1);
        lat2 = ToRad(lat2);

        var y = Math.Sin(dLon) * Math.Cos(lat2);
        var x = Math.Cos(lat1) * Math.Sin(lat2) -
                Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
        var bearing = Math.Atan2(y, x) * 180.0 / Math.PI;
        return (bearing + 360.0) % 360.0;
    }
} 