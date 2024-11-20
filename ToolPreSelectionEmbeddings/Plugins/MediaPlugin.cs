using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class MediaPlugin
{
    [KernelFunction, Description("Extracts MIME type from file extension")]
    public string GetMimeType(
        [Description("The file extension (e.g., .jpg, .pdf)")] string extension)
    {
        return extension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".doc" or ".docx" => "application/msword",
            ".xls" or ".xlsx" => "application/vnd.ms-excel",
            ".zip" => "application/zip",
            _ => "application/octet-stream"
        };
    }

    [KernelFunction, Description("Validates image dimensions")]
    public bool ValidateImageDimensions(
        [Description("Image width in pixels")] int width,
        [Description("Image height in pixels")] int height,
        [Description("Maximum allowed width")] int maxWidth = 1920,
        [Description("Maximum allowed height")] int maxHeight = 1080)
    {
        return width <= maxWidth && height <= maxHeight && width > 0 && height > 0;
    }

    [KernelFunction, Description("Calculates aspect ratio")]
    public string CalculateAspectRatio(
        [Description("Width value")] int width,
        [Description("Height value")] int height)
    {
        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        int gcd = GCD(width, height);
        return $"{width/gcd}:{height/gcd}";
    }
} 