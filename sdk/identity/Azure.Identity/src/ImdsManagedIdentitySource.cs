﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity
{
    internal class ImdsManagedIdentitySource : ManagedIdentitySource
    {
        // IMDS constants. Docs for IMDS are available here https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/how-to-use-vm-token#get-a-token-using-http
        private static readonly Uri s_imdsEndpoint = new Uri("http://169.254.169.254/metadata/identity/oauth2/token");
        internal const string imddsTokenPath = "/metadata/identity/oauth2/token";

        private const string ImdsApiVersion = "2018-02-01";

        internal const string IdentityUnavailableError = "ManagedIdentityCredential authentication unavailable. The requested identity has not been assigned to this resource.";
        internal const string NoResponseError = "ManagedIdentityCredential authentication unavailable. No response received from the managed identity endpoint.";
        internal const string TimeoutError = "ManagedIdentityCredential authentication unavailable. The request to the managed identity endpoint timed out.";
        internal const string GatewayError = "ManagedIdentityCredential authentication unavailable. The request failed due to a gateway error.";
        internal const string AggregateError = "ManagedIdentityCredential authentication unavailable. Multiple attempts failed to obtain a token from the managed identity endpoint.";

        private readonly string _clientId;
        private readonly Uri _imdsEndpoint;

        internal ImdsManagedIdentitySource(CredentialPipeline pipeline, string clientId) : base(pipeline)
        {
            _clientId = clientId;

            if (!string.IsNullOrEmpty(EnvironmentVariables.PodIdentityEndpoint))
			{
				var builder = new UriBuilder(EnvironmentVariables.PodIdentityEndpoint);
            	builder.Path = imddsTokenPath;
                _imdsEndpoint = builder.Uri;
			}
			else
			{
            	_imdsEndpoint = s_imdsEndpoint;
			}
        }

        protected override Request CreateRequest(string[] scopes)
        {
            // covert the scopes to a resource string
            string resource = ScopeUtilities.ScopesToResource(scopes);

            Request request = Pipeline.HttpPipeline.CreateRequest();
            request.Method = RequestMethod.Get;
            request.Headers.Add("Metadata", "true");
            request.Uri.Reset(_imdsEndpoint);
            request.Uri.AppendQuery("api-version", ImdsApiVersion);

            request.Uri.AppendQuery("resource", resource);

            if (!string.IsNullOrEmpty(_clientId))
            {
                request.Uri.AppendQuery("client_id", _clientId);
            }

            return request;
        }

        public async override ValueTask<AccessToken> AuthenticateAsync(bool async, TokenRequestContext context, CancellationToken cancellationToken)
        {
            try
            {
                return await base.AuthenticateAsync(async, context, cancellationToken).ConfigureAwait(false);
            }
            catch (RequestFailedException e) when (e.Status == 0)
            {
                throw new CredentialUnavailableException(NoResponseError, e);
            }
            catch (TaskCanceledException e)
            {
                throw new CredentialUnavailableException(NoResponseError, e);
            }
            catch (AggregateException e)
            {
                throw new CredentialUnavailableException(AggregateError, e);
            }
        }

        protected override async ValueTask<AccessToken> HandleResponseAsync(bool async, TokenRequestContext context, Response response, CancellationToken cancellationToken)
        {
            // handle error status codes indicating managed identity is not available
            var baseMessage = response.Status switch
            {
                400 => IdentityUnavailableError,
                502 => GatewayError,
                504 => GatewayError,
                _ => default(string)
            };

            if (baseMessage != null)
            {
                string message = await Pipeline.Diagnostics.CreateRequestFailedMessageAsync(response, baseMessage, null, null, async).ConfigureAwait(false);

                var errorContentMessage = await GetMessageFromResponse(response, async, cancellationToken).ConfigureAwait(false);

                if (errorContentMessage != null)
                {
                    message = message + Environment.NewLine + errorContentMessage;
                }

                throw new CredentialUnavailableException(message);
            }

            return await base.HandleResponseAsync(async, context, response, cancellationToken).ConfigureAwait(false);
        }
    }
}
