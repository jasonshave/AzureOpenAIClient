﻿// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        /// <param name="completionRequest"></param>
        /// <returns>A <see cref="CompletionResponse"/>.</returns>
        public async ValueTask<CompletionResponse?> GetTextCompletionResponseAsync(CompletionRequest completionRequest)
        {
            completionRequest.Stream = false;
            CompletionResponse? response = default;
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _openAiHttpClient.HttpClient.BaseAddress);

                var jsonRequest = JsonSerializer.Serialize(completionRequest, new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                });
                httpRequest.Content = new StringContent(jsonRequest);
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseMessage = await _openAiHttpClient.HttpClient.SendAsync(httpRequest);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var stringResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    response = JsonSerializer.Deserialize<CompletionResponse>(stringResult, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return response;
        }

        /// <summary>
        /// Use this method to send an OpenAI <see cref="CompletionRequest"/> and get back a streamed response.
        /// </summary>
        /// <param name="completionRequest"></param>
        /// <returns>Streamed <see cref="IAsyncEnumerable{T}"/> response.</returns>
        public async IAsyncEnumerable<CompletionResponse?> StreamTextCompletionResponseAsync(CompletionRequest completionRequest)
        {
            completionRequest.Stream = true;
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _openAiHttpClient.HttpClient.BaseAddress);

            var jsonRequest = JsonSerializer.Serialize(completionRequest, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            });
            httpRequest.Content = new StringContent(jsonRequest);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var responseMessage = await _openAiHttpClient.HttpClient.SendAsync(httpRequest);
            if (responseMessage.Content.Headers.ContentType.MediaType == "text/event-stream")
            {
                await using var stream = await responseMessage.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(line) && line != "data: [DONE]")
                    {
                        var formattedLine = line.Replace("data:", "");
                        var response = JsonSerializer.Deserialize<CompletionResponse>(formattedLine, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true,
                        });

                        yield return response;
                    }
                }
            }
        }
    }
}