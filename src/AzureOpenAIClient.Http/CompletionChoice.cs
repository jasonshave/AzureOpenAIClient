// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureOpenAIClient.Http
{
    public sealed class CompletionChoice
    {
        /// <summary>
        /// The API text returned from the request.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The index of completion choices
        /// </summary>
        public int Index { get; set; }

        public object LogProbs { get; set; }

        /// <summary>
        /// The reason for the completion choice.
        /// </summary>
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }
}