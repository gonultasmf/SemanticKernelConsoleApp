using System.ComponentModel;
using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
 
var builder = Kernel.CreateBuilder()
    .AddOllamaChatCompletion("deepseek-r1:latest", "http://localhost:11434");
builder.Plugins.AddFromType<CalculatorPlugin>();
builder.Services.AddScoped<HttpClient>();
 
var kernel = builder.Build();
 
var history = new ChatHistory();
//İlk mesaj eklenmektedir.
history.AddUserMessage("Merhaba! Bu gün hava nasıl?");
 
//Model çağrısı yapılmaktadır.
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var response = await chatCompletionService.GetChatMessageContentAsync(history);
 
//Response history'e eklenmektedir.
history.AddAssistantMessage(response.ToString());
 
//Aynı şekilde yeni mesajda history'e eklenerek devam edilir.
history.AddUserMessage("Peki, hafta sonu için hava tahmini nedir?");
var response2 = await chatCompletionService.GetChatMessageContentAsync(history);
history.AddAssistantMessage(response2.ToString());
 
Console.WriteLine(response2.ToString());

var arguments = new KernelArguments
{
    ["number1"] = 3,
    ["number2"] = 5
};
 
var addResult = await kernel.InvokeAsync("CalculatorPlugin", "add", arguments);
Console.WriteLine($"Sonuç : {addResult}");
 
Console.Read();

public class CalculatorPlugin
{
    [KernelFunction("add")]
    [Description("İki sayısal değer üzerinde toplama işlemi gerçekleştirir.")]
    [return: Description("Toplam değeri döndürür.")]
    public int Add(int number1, int number2)
        => number1 + number2;
}