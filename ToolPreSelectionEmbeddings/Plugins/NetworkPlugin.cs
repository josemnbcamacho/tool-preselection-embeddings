using System.ComponentModel;
using System.Net;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class NetworkPlugin
{
    [KernelFunction, Description("Extracts domain from URL")]
    public string ExtractDomain(
        [Description("The URL to process")] string url)
    {
        return new Uri(url).Host;
    }

    [KernelFunction, Description("Checks if port number is valid")]
    public bool ValidatePort(
        [Description("The port number to validate")] int port)
    {
        return port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort;
    }

    [KernelFunction, Description("Validates IPv4 address")]
    public bool ValidateIPv4(
        [Description("The IP address to validate")] string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out var ip) && 
               ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
    }
} 