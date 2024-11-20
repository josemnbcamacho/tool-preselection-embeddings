using Microsoft.Extensions.VectorData;

namespace ToolPreSelectionEmbeddings.Tools;

public class Tool
{
    [VectorStoreRecordKey]
    public string Key { get; set; } = string.Empty;

    [VectorStoreRecordData(IsFilterable = true)]
    public string Name { get; set; } = string.Empty;
    
    [VectorStoreRecordData(IsFilterable = true)]
    public string PluginName { get; set; } = string.Empty;
    

    [VectorStoreRecordData]
    public string Description { get; set; } = string.Empty;

    [VectorStoreRecordVector(Dimensions: 1536)]
    public ReadOnlyMemory<float> DescriptionEmbedding { get; set; }
}