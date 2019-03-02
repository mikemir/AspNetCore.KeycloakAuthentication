using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.KeycloakAuthentication.Clients
{
    public interface IKeycloakClient
    {
        Task<KeycloakToken> GetToken(string clientId, string secretKey);

        Task<KeycloakToken> GetToken(string clientId, string secretKey, string refreshToken);

        Task<KeycloakToken> GetTokenForUser(string userName, string password, string clientId, string secretKey);

    }
}
