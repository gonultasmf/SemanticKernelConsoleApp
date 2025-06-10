using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
 
var builder = Kernel.CreateBuilder()
    .AddOllamaChatCompletion("deepseek-r1:latest", "http://localhost:11434");
 
builder.Services.AddScoped<HttpClient>();
 
var kernel = builder.Build();
 
while (true)
{
    Console.WriteLine("Soru sor :");
    var input = Console.ReadLine();
    if (input != null)
    {
        var response = await kernel.InvokePromptAsync(input);
        Console.WriteLine($"Cevap :\n------------------------\n{response}\n------------------------");
    }
}