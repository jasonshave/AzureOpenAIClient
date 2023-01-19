// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;

namespace AzureOpenAIClient.Http
{
    public sealed class OpenAIHttpClient
    {
        public HttpClient HttpClient { get; }

        public OpenAIHttpClient(HttpClient httpClient, OpenAIClientConfiguration openAiClientConfiguration)
        {
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri($"{openAiClientConfiguration.BaseUri}openai/deployments/{openAiClientConfiguration.DeploymentName}/completions?api-version={openAiClientConfiguration.ApiVersion}");
            HttpClient.DefaultRequestHeaders.Add("api-key", openAiClientConfiguration.ApiKey);
        }
    }
}