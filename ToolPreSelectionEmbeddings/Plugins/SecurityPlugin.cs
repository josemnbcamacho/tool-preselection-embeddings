using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class SecurityPlugin
{
    [KernelFunction, Description("Generates secure hashes")]
    public string HashString(
        [Description("The string to hash")] string input,
        [Description("The hash algorithm to use (SHA256, SHA512, MD5)")] string algorithm = "SHA256")
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = algorithm.ToUpper() switch
        {
            "SHA256" => SHA256.HashData(bytes),
            "SHA512" => SHA512.HashData(bytes),
            "MD5" => MD5.HashData(bytes),
            _ => throw new ArgumentException("Unsupported hash algorithm")
        };
        return Convert.ToHexString(hash);
    }

    [KernelFunction, Description("Generates a random password")]
    public string GeneratePassword(
        [Description("The length of the password")] int length = 16,
        [Description("Include special characters")] bool includeSpecial = true)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        
        var chars = letters + digits + (includeSpecial ? special : "");
        var result = new char[length];
        
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length * 4];
        rng.GetBytes(bytes);
        
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[BitConverter.ToInt32(bytes, i * 4) % chars.Length];
        }
        
        return new string(result);
    }
} 