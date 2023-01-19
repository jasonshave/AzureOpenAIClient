// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace AzureOpenAIClient.Http
{
    public sealed class CompletionResponse
    {
        /// <summary>
        /// The ID of the API call
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The completion type
        /// </summary>
        public string Object { get; set; }

        /// <summary>
        /// Completed on (epoch time)
        /// </summary>
        public int Created { get; set; }

        /// <summary>
        /// The model used in the request.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// A list of responses.
        /// </summary>
        public List<CompletionChoices> Choices { get; set; } = new List<CompletionChoices>();
    }
}