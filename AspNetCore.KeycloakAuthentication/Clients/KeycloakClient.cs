using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.KeycloakAuthentication.Clients
{
    public class KeycloakToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class KeycloakClient : IKeycloakClient
    {
        private readonly HttpClient _httpClient;

        public KeycloakClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<KeycloakToken> GetToken(string clientId, string secretKey)
        {
            KeycloakToken result = null;
            var grantType = "client_credentials";
            var url = $"{_httpClient.BaseAddress}/protocol/openid-connect/token";

            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", secretKey),
                    new KeyValuePair<string, string>("grant_type", grantType)
                })
            };


            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<KeycloakToken>(jsonString);
            }

            return result;
        }

        public async Task<KeycloakToken> GetToken(string clientId, string secretKey, string refreshToken)
        {
            KeycloakToken result = null;
            var grantType = "refresh_token";
            var url = $"{_httpClient.BaseAddress}/protocol/openid-connect/token";

            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", secretKey),
                    new KeyValuePair<string, string>("grant_type", grantType),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)
                })
            };


            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<KeycloakToken>(jsonString);
            }

            return result;
        }

        public async Task<KeycloakToken> GetTokenForUser(string userName, string password, string clientId, string secretKey)
        {
            KeycloakToken result = null;
            var grantType = "client_credentials";
            var url = $"{_httpClient.BaseAddress}/protocol/openid-connect/token";

            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", secretKey),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", grantType)
                })
            };


            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<KeycloakToken>(jsonString);
            }

            return result;
        }
    }
}
