using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class MathPlugin
{
    [KernelFunction, Description("Performs mathematical calculations")]
    public double Calculate([Description("The mathematical expression to evaluate")] string expression)
    {
        try
        {
            // Note: In a production environment, you'd want to use a proper expression parser
            // This is a simplified example using DataTable.Compute() or similar
            return Convert.ToDouble(new System.Data.DataTable().Compute(expression, null));
        }
        catch (Exception)
        {
            throw new ArgumentException($"Unable to evaluate expression: {expression}");
        }
    }
} 