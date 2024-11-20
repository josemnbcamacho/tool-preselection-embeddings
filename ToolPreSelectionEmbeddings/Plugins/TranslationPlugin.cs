using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class TranslationPlugin
{
    [KernelFunction, Description("Translates text between languages")]
    public async Task<string> Translate(
        [Description("The text to translate")] string text,
        [Description("The source language code (e.g., 'en')")] string fromLanguage,
        [Description("The target language code (e.g., 'es')")] string toLanguage)
    {
        // In a real implementation, you would integrate with a translation API
        await Task.Delay(100); // Simulate API call
        return $"Translated '{text}' from {fromLanguage} to {toLanguage} (mock implementation)";
    }
} 