// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenAi.Http.Client
{
    public sealed class CompletionRequest
    {
        /// <summary>
        /// The prompt(s) to generate completions for, encoded as a string, a list of strings, or a list of token lists. Note that |endoftext| is the document separator that the model sees during training, so if a prompt isn't specified the model will generate as if from the beginning of a new document.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// The maximum number of tokens to generate in the completion. The token count of your prompt plus max_tokens can't exceed the model's context length. Most models have a context length of 2048 tokens (except davinci-codex, which supports 4096).
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }

        /// <summary>
        /// What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer. We generally recommend altering this or top_p but not both.
        /// </summary>
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. We generally recommend altering this or temperature but not both.
        /// </summary>
        [JsonPropertyName("top_p")]
        public double? TopP { get; set; }

        /// <summary>
        /// How many completions to generate for each prompt. Note: Because this parameter generates many completions, it can quickly consume your token quota. Use carefully and ensure that you have reasonable settings for max_tokens and stop.
        /// </summary>
        [JsonPropertyName("n")]
        public string? NumberOfCompletionsToGenerate { get; set; }

        /// <summary>
        /// Whether to stream back partial progress. If set, tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a data: [DONE] message.
        /// </summary>
        [JsonPropertyName("stream")]
        public bool? Stream { get; set; }

        /// <summary>
        /// Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens. For example, if logprobs is 10, the API will return a list of the 10 most likely tokens. the API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.
        /// </summary>
        [JsonPropertyName("logprobs")]
        public int? LogProbabilities { get; set; }

        /// <summary>
        /// Echo back the prompt in addition to the completion
        /// </summary>
        [JsonPropertyName("echo")]
        public bool? Echo { get; set; }

        /// <summary>
        /// Up to four sequences where the API will stop generating further tokens. The returned text won't contain the stop sequence.
        /// </summary>
        public string? StopText { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
        /// </summary>
        public double? PresencePenalty { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        public double? FrequencyPenalty { get; set; }

        /// <summary>
        /// Generates best_of completions server-side and returns the "best" (the one with the lowest log probability per token). Results can't be streamed. When used with n, best_of controls the number of candidate completions and n specifies how many to return – best_of must be greater than n. Note: Because this parameter generates many completions, it can quickly consume your token quota. Use carefully and ensure that you have reasonable settings for max_tokens and stop.
        /// </summary>
        public int? BestOf { get; set; }

        /// <summary>
        /// Modify the likelihood of specified tokens appearing in the completion. Accepts a json object that maps tokens (specified by their token ID in the GPT tokenizer) to an associated bias value from -100 to 100. You can use this tokenizer tool (which works for both GPT-2 and GPT-3) to convert text to token IDs. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result in a ban or exclusive selection of the relevant token. As an example, you can pass {"50256": -100} to prevent the <|endoftext|> token from being generated.
        /// </summary>
        public KeyValuePair<string, int>? LogitBias { get; set; }
    }
}