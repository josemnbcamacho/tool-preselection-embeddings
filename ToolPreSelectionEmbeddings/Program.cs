using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.InMemory;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ToolPreSelectionEmbeddings.Benchmarks;
using ToolPreSelectionEmbeddings.Plugins;
using ToolPreSelectionEmbeddings.Tools;
using Microsoft.Extensions.Configuration;

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only
#pragma warning disable SKEXP0003 // Type is for evaluation purposes only
#pragma warning disable SKEXP0004 // Type is for evaluation purposes only
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only
#pragma warning disable SKEXP0011 // Type is for evaluation purposes only
#pragma warning disable SKEXP0021 // Type is for evaluation purposes only
#pragma warning disable SKEXP0026 // Type is for evaluation purposes only
#pragma warning disable SKEXP0028 // Type is for evaluation purposes only
#pragma warning disable SKEXP0050 // Type is for evaluation purposes only

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", true)
            .Build();

        var openAiConfig = configuration.GetSection("AzureOpenAI");

        // Initialize Semantic Kernel
        var builder = Kernel.CreateBuilder();
        
        // Add Azure OpenAI chat completion capability
        builder.AddAzureOpenAIChatCompletion(
            openAiConfig["Model"] ?? "gpt-4o-mini",
            openAiConfig["Endpoint"] ?? throw new ArgumentNullException("AzureOpenAI:Endpoint"),
            openAiConfig["Key"] ?? throw new ArgumentNullException("AzureOpenAI:Key"),
            openAiConfig["ServiceId"] ?? "azure-openai"
        );
        
        // Add embedding generation capability
        builder.AddAzureOpenAITextEmbeddingGeneration(
            openAiConfig["EmbeddingModel"] ?? "text-embedding-ada-002",
            openAiConfig["Endpoint"] ?? throw new ArgumentNullException("AzureOpenAI:Endpoint"),
            openAiConfig["Key"] ?? throw new ArgumentNullException("AzureOpenAI:Key"),
            openAiConfig["ServiceId"] ?? "azure-openai"
        );
        
        // Add vector store
        var vectorStore = new InMemoryVectorStore();
        builder.Services.AddSingleton<IVectorStore>(_ => vectorStore);
        
        var kernel = builder.Build();
        
        // Initialize ToolPreSelectionEmbeddings components with plugins
        var plugins = RegisterPlugins(kernel);
        var toolRegistry = new ToolRegistry(vectorStore, kernel.GetRequiredService<ITextEmbeddingGenerationService>());
        var descriptionGenerator = new DescriptionGenerator(kernel);
        
        // Register the plugins' functions
        await RegisterPluginFunctions(toolRegistry, plugins);
        
        // Simple interaction loop
        while (true)
        {
            Console.WriteLine("\nEnter your request (or 'exit' to quit):");
            var userInput = Console.ReadLine();
            
            if (string.IsNullOrEmpty(userInput) || string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
                break;

            if (string.Equals(userInput, "-benchmark", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nWarning: Running benchmarks will incur API costs. Do you want to continue? (y/n)");
                var confirmation = Console.ReadLine();
                if (string.Equals(confirmation, "y", StringComparison.OrdinalIgnoreCase))
                {
                    await BenchmarkRunner.RunBenchmark(toolRegistry, descriptionGenerator);
                }
                continue;
            }

            try
            {
                // Benchmark both approaches
                Console.WriteLine("\n=== Running Benchmark ===");
                
                // With HyDE
                var hydeStartTime = DateTime.UtcNow;
                var hypotheticalDescription = await descriptionGenerator.GenerateDescription(userInput);
                var hydeMatches = await ToolMatcher.FindBestMatch(hypotheticalDescription, toolRegistry);
                var hydeDuration = DateTime.UtcNow - hydeStartTime;
                
                // Without HyDE (direct matching)
                var directStartTime = DateTime.UtcNow;
                var directMatches = await ToolMatcher.FindBestMatch(userInput, toolRegistry);
                var directDuration = DateTime.UtcNow - directStartTime;
                
                // Print benchmark results
                Console.WriteLine("\nBenchmark Results:");
                Console.WriteLine($"HyDE Approach: {hydeDuration.TotalMilliseconds:F2}ms");
                Console.WriteLine($"Direct Approach: {directDuration.TotalMilliseconds:F2}ms");
                
                Console.WriteLine("\nHyDE Matches:");
                BenchmarkRunner.PrintMatches(hydeMatches);
                
                Console.WriteLine("\nDirect Matches:");
                BenchmarkRunner.PrintMatches(directMatches);

                // Use HyDE matches for execution (original behavior)
                if (hydeMatches.Any())
                {
                    // Get all matching functions and add them to allowed tools
                    var allowedTools = hydeMatches
                        .Select(match => kernel.Plugins.GetFunction(match.PluginName, match.Name));

                    // Execute with only the matched functions
                    var result = await kernel.InvokePromptAsync(userInput,
                        new KernelArguments(new OpenAIPromptExecutionSettings
                        {
                            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(allowedTools),
                            ModelId = "gpt-4o-mini",
                            ServiceId = "azure-openai"
                        }));
                    
                    Console.WriteLine($"\nResult: {result.GetValue<string>()}");
                }
                else
                {
                    Console.WriteLine("\nNo matching tool found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static KernelPluginCollection RegisterPlugins(Kernel kernel)
    {
        kernel.Plugins.AddFromType<TimePlugin>();
        kernel.Plugins.AddFromType<MathPlugin>();
        kernel.Plugins.AddFromType<WeatherPlugin>();
        kernel.Plugins.AddFromType<TranslationPlugin>();
        kernel.Plugins.AddFromObject(new TextPlugin(kernel));
        kernel.Plugins.AddFromType<FilePlugin>();
        kernel.Plugins.AddFromType<ValidationPlugin>();
        kernel.Plugins.AddFromType<ConversionPlugin>();
        kernel.Plugins.AddFromType<FormatPlugin>();
        kernel.Plugins.AddFromType<SecurityPlugin>();
        kernel.Plugins.AddFromType<UtilityPlugin>();
        kernel.Plugins.AddFromType<DataAnalysisPlugin>();
        kernel.Plugins.AddFromType<StringManipulationPlugin>();
        kernel.Plugins.AddFromType<NetworkPlugin>();
        kernel.Plugins.AddFromType<MediaPlugin>();
        kernel.Plugins.AddFromType<FinancePlugin>();
        kernel.Plugins.AddFromType<GeographyPlugin>();
        
        return kernel.Plugins;
    }

    private static async Task RegisterPluginFunctions(ToolRegistry registry, KernelPluginCollection plugins)
    {
        foreach (var plugin in plugins)
        {
            foreach (var functionMetadata in plugin.GetFunctionsMetadata())
            {
                await registry.RegisterTool(
                    functionMetadata.Name,
                    plugin.Name,
                    functionMetadata.Description
                );
            }
        }
    }
}

public class DescriptionGenerator
{
    private readonly Kernel _kernel;
    private const string DescriptionPrompt = @"
Based on the user's request, generate a brief, focused description of a tool that would handle this request.
Use these examples as a guide:

Example requests and their tool descriptions:
1. Request: 'Send an email to John'
   Description: Sends emails to specified recipients with customizable content

2. Request: 'Convert 5 meters to feet'
   Description: Converts values between different measurement units

3. Request: 'Set a reminder for tomorrow at 2pm'
   Description: Creates time-based reminders with custom messages

4. Request: 'Generate a random password'
   Description: Generates secure random passwords with configurable options

5. Request: 'Compress this image'
   Description: Compresses images while preserving quality

Request: {{$input}}
Description:";

    public DescriptionGenerator(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<string> GenerateDescription(string userInput)
    {
        OpenAIPromptExecutionSettings executionSettings = new()
        {
            MaxTokens = 100,
            ModelId = "gpt-4o-mini",
            ServiceId = "azure-openai",
            Temperature = 0.2
        };
        
        var arguments = new KernelArguments(executionSettings)
        {
            ["input"] = userInput
        };
        
        var result = await _kernel.InvokePromptAsync(DescriptionPrompt, arguments);
        return result.GetValue<string>() ?? string.Empty;
    }
}