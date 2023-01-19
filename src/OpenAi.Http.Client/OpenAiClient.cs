// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAi.Http.Client
{
    public sealed class OpenAiClient
    {
        private readonly OpenAiHttpClient _openAiHttpClient;
        private readonly ILogger<OpenAiClient> _logger;

        public OpenAiClient(OpenAiHttpClient openAiHttpClient, ILogger<OpenAiClient> logger)
        {
            _openAiHttpClient = openAiHttpClient;
            _logger = logger;
        }

        public async ValueTask<CompletionResponse?> GetTextCompletionResponse(CompletionRequest input)
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

                    return response;
                }

                throw new ApplicationException(
                    $"There was a problem processing the request: {responseMessage.ReasonPhrase}");

            }
            catch (ApplicationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}