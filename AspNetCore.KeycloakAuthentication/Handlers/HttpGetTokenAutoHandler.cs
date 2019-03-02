using AspNetCore.KeycloakAuthentication.Clients;
using AspNetCore.KeycloakAuthentication.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.KeycloakAuthentication.Handlers
{
    public class HttpGetTokenAutoHandler : DelegatingHandler
    {
        private readonly IKeycloakClient _keycloakClient;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly KeycloakOptions _options;

        public HttpGetTokenAutoHandler(IKeycloakClient keycloakClient, 
                                       IHttpContextAccessor httpContextAccesor, 
                                       KeycloakOptions options)
        {
            _keycloakClient = keycloakClient;
            _httpContextAccesor = httpContextAccesor;
            _options = options;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Authorization"))
            {
                var token = await GetToken();

                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<KeycloakToken> GetToken()
        {
            var keyToken = "token";

            KeycloakToken resultToken = null;
            var tokenSession = _httpContextAccesor.HttpContext.Session.GetString(keyToken); //ToDo: Cambiar por un sessionTokenManager
            
            if (string.IsNullOrEmpty(tokenSession))
            {
                resultToken = await _keycloakClient.GetToken(_options.ClientId, _options.ClientSecret);                
            }
            else
            {
                resultToken = JsonConvert.DeserializeObject<KeycloakToken>(tokenSession);

                if (_options.EnableRefreshToken)
                {
                    var unixTime = new DateTimeOffset(DateTime.Now);
                    var jwt = new JwtSecurityToken(resultToken.AccessToken);

                    if (jwt.Payload.Exp.HasValue && unixTime.ToUnixTimeSeconds() > jwt.Payload.Exp.Value)
                    {
                        resultToken = await _keycloakClient.GetToken(_options.ClientId, _options.ClientSecret, resultToken.RefreshToken);
                    } 
                }
            }

            _httpContextAccesor.HttpContext.Session.SetString(keyToken, JsonConvert.SerializeObject(resultToken));

            return resultToken;
        }
    }
}
