using System.Text.Json.Serialization;

namespace OpenAi.Http.Client
{
    public sealed class CompletionChoices
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