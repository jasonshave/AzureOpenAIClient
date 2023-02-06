# Azure OpenAI .NET Client

This project contains an implementation of a pre-configured injectable `OpenAIClient` which can be used to communicate to the Azure OpenAI platform instead of relying purely on hand-crafted REST API calls.

## OpenAIClient settings

The following settings allow the OpenAI client to connect to your Azure resource, authenticate, and work with the right deployment model you've created in the portal.

| Setting | Purpose | Example |
|---|---|---|
| BaseUri | The fully qualified domain name of the OpenAI resource in Azure. | `https://myai.openai.azure.com/` |
| ApiKey | The API key used to authenticate the client to OpenAI in Azure. | `1bbcc11a3a233857zz12aa5f2fake99af7d9c` |
| DeploymentName | The name of the deployment for a given OpenAI model. | `text-davinci-002` |
| ApiVersion | The API version for your OpenAI resource. | `2022-12-01` |

## Configuration

The section below shows an example JSON configuration which can be stored in your `secrets.json` file, `appsettings.development.json`, or `appsettings.json`.

```json
"OpenAiClientConfiguration": {
    "BaseUri": "[your_fqdn]",
    "ApiKey": "[your_api_key]",
    "DeploymentName": "[your_deployment_name]",
    "ApiVersion": "[supported_api_version]"
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
    private readonly OpenAIClient _openAiClient;

    public MyClass(OpenAIClient client)
    {
        _openAiClient = client;
    }

    Task DoWork(string input)
    {
        var completionRequest = new CompletionRequest()
            {
                Prompt = input,
                MaxTokens = 100
            };
        CompletionResponse? completionResponse = await _openAiClient.GetTextCompletionResponseAsync(completionRequest);            
    }        
}
```
## Community Links

[Blazor and Azure OpenAI](https://blazorhelpwebsite.com/ViewBlogPost/2065)
