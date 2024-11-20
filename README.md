# Tool Pre-Selections Embeddings

This project is a C# application built with Microsoft's Semantic Kernel that implements a technique to pre-select tools based on embeddings similarity. This is a proof of concept to be used in an LLM Agent to improve the performance and the quality of the responses. This allows for an LLM Agent to have a much larger set of tools (100+) and still provide good performance, without having the costs of sending all the tools to the LLM call.

It starts by creating a semantic embedding representation of the available tools. Then, when a user request is received, it creates an embedding for the request and compares it to the embeddings of the available tools. The tools with the highest similarity score are selected and used to execute the request.

In order to improve the performance of similarity search, the project also implements the Hypothetical Document Embeddings (HyDE) technique. This technique creates an hypothetical tool description from the user request and then creates an embedding for this hypothetical tool. This embedding is compared to the embeddings of the available tools, and the tools with the highest similarity score are selected.

## Features

- **Intelligent Tool Matching**: Uses Hypothetical Document Embeddings (HyDE) to match user requests with appropriate tools
- **Benchmarking**: Compare performance between direct matching and HyDE approaches
- **Benchmark Data**: A set of queries with the expected tool calls is available at [BenchmarkData.cs](./ToolPreSelectionEmbeddings/Benchmarks/BenchmarkData.cs)

## Prerequisites

- .NET 8.0
- Azure OpenAI API access
- Visual Studio 2022 or compatible IDE

## Configuration

1. Create an `appsettings.local.json` file with your Azure OpenAI credentials: 

## Getting Started

1. Clone the repository
2. Configure your Azure OpenAI credentials in `appsettings.local.json`
3. Build and run the project:
```bash
dotnet build
dotnet run
```

## Usage

The application provides an interactive console interface where you can:

1. Enter natural language requests to execute tools
2. Run benchmarks using the `-benchmark` command
3. Exit using 'exit' command

Example interactions:
```bash
Enter your request (or 'exit' to quit):
> convert 5 meters to feet

Enter your request (or 'exit' to quit):
> -benchmark

Enter your request (or 'exit' to quit):
> exit
```

## Benchmark Results

With the current set of plugins, using ´gpt-4o-mini´ as the LLM, the benchmark results are:
- Direct Matching: 90% accuracy, ~200ms per request
- HyDE: 100% accuracy, ~1000ms per request

## Plugin Categories

1. **Time & Date**
   - Current time retrieval
   - Time difference calculations
   - Business hours validation
   - Age calculations

2. **Data Processing**
   - Text analysis
   - Statistical calculations
   - Format conversions
   - Data validation

3. **Security**
   - Hash generation
   - Password validation
   - Secure random generation

4. **Utilities**
   - File operations
   - String manipulation
   - Network utilities
   - Geographic calculations

## License

Licensed under the [MIT License](LICENSE).

## Acknowledgments

- My good friend [AndreBaltazar8](https://github.com/AndreBaltazar8) for the initial idea. Checkout his [repo](https://github.com/AndreBaltazar8/tool-pre-selections) for a extensive description of this technique.
- Built with [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel). I'm a huge fan of this project and I think it's a game changer for the future of AI Agents.
- [Precise Zero-Shot Dense Retrieval without Relevance Labels](https://arxiv.org/abs/2212.10496) paper for the HyDE technique.
