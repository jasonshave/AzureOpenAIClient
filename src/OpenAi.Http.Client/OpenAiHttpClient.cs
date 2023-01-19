// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;

namespace OpenAi.Http.Client
{
    public sealed class OpenAiHttpClient
    {
        public HttpClient HttpClient { get; }

        public OpenAiHttpClient(HttpClient httpClient, OpenAiClientConfiguration openAiClientConfiguration)
        {
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri($"{openAiClientConfiguration.BaseUri}openai/deployments/{openAiClientConfiguration.DeploymentName}/completions?api-version={openAiClientConfiguration.ApiVersion}");
            HttpClient.DefaultRequestHeaders.Add("api-key", openAiClientConfiguration.ApiKey);
        }
    }
}