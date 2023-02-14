# Azure OpenAI .NET Client

This project contains a .NET client library for use with Azure's OpenAI resource. It contains an implementation of a pre-configured injectable `OpenAIClient` which can be used to communicate to the Azure OpenAI platform instead of relying purely on hand-crafted REST API calls.

> NOTE: For the official Azure.AI.OpenAI NuGet package, please use [this one](https://www.nuget.org/packages/Azure.AI.OpenAI).

## OpenAIClient settings

The following settings allow the OpenAI client to connect to your Azure resource, authenticate, and work with the right deployment model you've created in the portal.

| Setting | Purpose | Example |
|---|---|---|
| BaseUri | The fully qualified domain name of the OpenAI resource in Azure. | `https://myai.openai.azure.com/` |
| ApiKey | The API key used to authenticate the client to OpenAI in Azure. | `1bbcc11a3a233857zz12aa5f2fake99af7d9c` |
| DeploymentName | The name of the deployment for a given OpenAI model. | `text-davinci-002` |

## Configuration

The section below shows an example JSON configuration which can be stored in your `secrets.json` file, `appsettings.development.json`, or `appsettings.json`.

```json
"OpenAiClientConfiguration": {
    "BaseUri": "[your_fqdn]",
    "ApiKey": "[your_api_key]",
    "DeploymentName": "[your_deployment_name]"
}
```

## Setup in Program.cs

In your .NET application, you can use the NuGet package to allow for the following extension method which will register a singleton reference of the `OpenAIClient` instance.

`builder.Services.AddOpenAIClient(x => builder.Configuration.Bind(nameof(OpenAIClientConfiguration), x));`

## OpenAIClient usage

Since the `OpenAIClient` is registered for Dependency Injection, you can inject it and use it with the `GetTextCompletionResponseAsync()` method. The `CompletionResponse` contains the model OpenAI uses with their typical REST API.

```csharp
public class MyClass
{
    private readonly OpenAIClient _client;

    public MyClass(OpenAIClient client)
    {
        _client = client;
    }

    Task DoWork(string input)
    {
        var completionRequest = new CompletionRequest()
        {
            Prompt = input,
            MaxTokens = 100
        };
        CompletionResponse? completionResponse = await _client.GetTextCompletionResponseAsync(completionRequest);
    }        
}
```

## Streaming support

The client now supports streaming using `IAsyncEnumerable<CompletionResponse?>` and can be iterated using the `await foreach` pattern as follows:

```csharp
// inject the OpenAIClient and call streaming method
var completionRequest = new CompletionRequest()
{
    Prompt = input,
    MaxTokens = 100
};
var stream = _client.StreamTextCompletionResponseAsync(completionRequest);

await foreach (var completionResponse in stream)
{
    Console.Write(completionResponse.Choices.FirstOrDefault().Text);
}
```

## Community Links

[Blazor and Azure OpenAI](https://blazorhelpwebsite.com/ViewBlogPost/2065)
