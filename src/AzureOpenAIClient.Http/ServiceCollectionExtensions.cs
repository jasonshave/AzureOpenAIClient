// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzureOpenAIClient.Http
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAiClient(this IServiceCollection services, string baseUri, string apiKey, string deploymentName, string apiVersion)
        {
            ConfigureCommon(services, new OpenAIClientConfiguration()
            {
                ApiKey = apiKey,
                BaseUri = baseUri,
                DeploymentName = deploymentName,
                ApiVersion = apiVersion
            });
            return services;
        }

        public static IServiceCollection AddOpenAiClient(this IServiceCollection services, Action<OpenAIClientConfiguration> option)
        {
            var openAiHttpClientConfiguration = new OpenAIClientConfiguration();
            option(openAiHttpClientConfiguration);

            ConfigureCommon(services, openAiHttpClientConfiguration);
            return services;
        }

        private static void ConfigureCommon(IServiceCollection services, OpenAIClientConfiguration openAiClientConfiguration)
        {
            services.AddSingleton(openAiClientConfiguration);
            services.AddSingleton<OpenAIClient>();

            services.AddHttpClient<OpenAIHttpClient>();
        }
    }
}