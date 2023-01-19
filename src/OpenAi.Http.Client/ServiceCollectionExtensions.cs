﻿// Copyright (c) 2022 Jason Shave. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace OpenAi.Http.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAiClient(this IServiceCollection services, string baseUri, string apiKey, string deploymentName, string apiVersion)
        {
            ConfigureCommon(services, new OpenAiClientConfiguration()
            {
                ApiKey = apiKey,
                BaseUri = baseUri,
                DeploymentName = deploymentName,
                ApiVersion = apiVersion
            });
            return services;
        }

        public static IServiceCollection AddOpenAiClient(this IServiceCollection services, Action<OpenAiClientConfiguration> option)
        {
            var openAiHttpClientConfiguration = new OpenAiClientConfiguration();
            option(openAiHttpClientConfiguration);

            ConfigureCommon(services, openAiHttpClientConfiguration);
            return services;
        }

        private static void ConfigureCommon(IServiceCollection services, OpenAiClientConfiguration openAiClientConfiguration)
        {
            services.AddSingleton(openAiClientConfiguration);
            services.AddSingleton<OpenAiClient>();

            services.AddHttpClient<OpenAiHttpClient>();
        }
    }
}