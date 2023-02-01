// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureOpenAIClient.Http
{
    public sealed class OpenAIClient
    {
        private readonly OpenAIHttpClient _openAiHttpClient;
        private readonly ILogger<OpenAIClient> _logger;

        public OpenAIClient(OpenAIHttpClient openAiHttpClient, ILogger<OpenAIClient> logger)
        {
            _openAiHttpClient = openAiHttpClient;
            _logger = logger;
        }

        /// <summary>
        /// Use this method to send an OpenAI <see cref="CompletionRequest"/> using the built-in <see cref="HttpClient"/>.
        /// Serialization and deserialization of the request is handled automatically.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A <see cref="CompletionResponse"/>.</returns>
        public async ValueTask<CompletionResponse?> GetTextCompletionResponseAsync(CompletionRequest input)
        {
            CompletionResponse? response = default;
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _openAiHttpClient.HttpClient.BaseAddress);

                var jsonRequest = JsonSerializer.Serialize(input, new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                httpRequest.Content = new StringContent(jsonRequest);
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await _openAiHttpClient.HttpClient.SendAsync(httpRequest);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var stringResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    response = JsonSerializer.Deserialize<CompletionResponse>(stringResult, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return response;
        }
    }
}