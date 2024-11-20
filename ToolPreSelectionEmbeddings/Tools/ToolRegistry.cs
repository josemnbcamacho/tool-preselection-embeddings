using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;

#pragma warning disable SKEXP0001

namespace ToolPreSelectionEmbeddings.Tools;

public class ToolRegistry
{
    private readonly ITextEmbeddingGenerationService _embeddingService;
    private readonly IVectorStoreRecordCollection<string,Tool> _collection;
    private const string CollectionName = "tools";

    public ToolRegistry(IVectorStore vectorStore, ITextEmbeddingGenerationService embeddingService)
    {
        _embeddingService = embeddingService;
        _collection = vectorStore.GetCollection<string, Tool>(CollectionName);
    }

    public async Task RegisterTool(string name, string pluginName, string description)
    {
        await _collection.CreateCollectionIfNotExistsAsync();

        var tool = new Tool
        {
            Key = Guid.NewGuid().ToString(),
            Name = name,
            PluginName = pluginName,
            Description = description,
            DescriptionEmbedding = await _embeddingService.GenerateEmbeddingAsync(description),
        };

        await _collection.UpsertAsync(tool);
    }

    public async Task<IEnumerable<(Tool Tool, double Score)>> SearchToolsAsync(string description)
    {
        var embedding = await _embeddingService.GenerateEmbeddingAsync(description);
        
        var results = await _collection.VectorizedSearchAsync(embedding,
            new()
            {
                Top = 5
            });

        return results.Results.ToBlockingEnumerable().Select(x => (x.Record, x.Score ?? 0));
    }
}