using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.KeycloakAuthentication.Configuration
{
    public class KeycloakOptions
    {
        /// <summary>
        ///     Define la URL completa para la instancia de Keycloak.
        /// </summary>
        /// <remarks>
        ///     Por defecto, Keycloak se implementa en el submódulo "/auth" 
        ///     en el servidor web, que debe incluirse en esta URL.
        /// </remarks>
        public string KeycloakUrl { get; set; } = string.Empty;

        /// <summary>
        ///     El reino de Keycloak en el que se encuentra el cliente.
        /// </summary>
        public string Realm { get; set; } = string.Empty;

        /// <summary>
        ///     El ID de cliente a utilizar para la aplicación.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     OPCIONAL: La secret key del cliente a usar para validar el token.
        /// </summary>
        /// <remarks>
        ///     - No requerido para clientes públicos o al portador (Bearer).
        ///     - Default: None
        /// </remarks>
        public string ClientSecret { get; set; }

        /// <summary>
        ///     OPCIONAL: El tiempo de gracia máximo para aceptar los tokens caducados.
        /// </summary>
        /// <remarks>
        ///     - Default: 0 seconds
        /// </remarks>
        public TimeSpan TokenClockSkew { get; set; } = TimeSpan.Zero;

        /// <summary>
        ///     OPCIONAL: Establecer en true para soportar el refresco automatico de tokens
        /// </summary>
        /// <remarks>
        ///     - Default: False
        /// </remarks>
        public bool EnableRefreshToken { get; set; } = false;

        /// <summary>
        ///     OPCIONAL: Establecer en true obtener un token de autenticación (JWT),
        ///     si no se posee un token guardado.
        /// </summary>
        /// <remarks>
        ///     - Default: False
        /// </remarks>
        public bool EnableGetTokenAuto { get; set; } = false;

        public string KeycloakUrlRealm => $"{NormalizeUrl(KeycloakUrl)}/realms/{Realm.ToLowerInvariant()}";

        private static string NormalizeUrl(string url)
        {
            var urlNormalized = !url.EndsWith('/') ? url : url.TrimEnd('/');

            return urlNormalized?.ToLowerInvariant();
        }
    }
}
