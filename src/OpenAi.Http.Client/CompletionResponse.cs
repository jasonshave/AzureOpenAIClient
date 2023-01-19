﻿using System.Collections.Generic;

namespace OpenAi.Http.Client
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