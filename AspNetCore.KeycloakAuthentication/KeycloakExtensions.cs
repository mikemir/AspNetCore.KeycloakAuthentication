using AspNetCore.KeycloakAuthentication.Clients;
using AspNetCore.KeycloakAuthentication.Configuration;
using AspNetCore.KeycloakAuthentication.Handlers;
using AspNetCore.KeycloakAuthentication.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AspNetCore.KeycloakAuthentication
{
    public static class KeycloakExtensions
    {
        private static bool _enableGetTokenAuto;
        private static bool _enableRefreshToken;

        /// <summary>
        /// Agrega y configura la funcionalidad de Autenticación y autorización de Keycloak a un proyecto .NET Core
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">Parametro con los valores necesarios para configurar la autenticación de Keycloak.</param>
        /// <returns>Devuelve una referencia a la instancia después de implementar la autenticación con JWT.</returns>
        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, KeycloakOptions options)
        {

            //Se agrega al contenedor de dependencias el KeycloakClient
            services.AddSingleton(options);
            //Se agrega al contenedor las opciones de configuración
            services.AddHttpClient<IKeycloakClient>(c => c.BaseAddress = new Uri(options.KeycloakUrlRealm))
                    .AddTypedClient<IKeycloakClient, KeycloakClient>();

            #region PASOS 1 Y 2 (ENVIO Y REFRESCO DE TOKEN)

            if(options.EnableGetTokenAuto || options.EnableRefreshToken)
            {
                services.AddHttpContextAccessor();
                services.AddSession(config => config.Cookie.Name = options.ClientId);

                if (options.EnableGetTokenAuto)
                {
                    _enableGetTokenAuto = true;
                    services.AddTransient<HttpGetTokenAutoHandler>();
                }

                if (options.EnableRefreshToken)
                {
                    _enableRefreshToken = true;
                }
            }

            #endregion

            #region PASOS 3 Y 4 (VALIDACION DE TOKEN)

            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = options.TokenClockSkew,
                ValidateAudience = true,
                ValidateIssuer = true
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opts =>
                    {
                        opts.Authority = options.KeycloakUrlRealm;
                        opts.Audience = options.ClientId;
                        opts.TokenValidationParameters = validationParameters;
                        opts.RequireHttpsMetadata = false;
                        opts.SaveToken = true;
                    });

            services.AddTransient<IAuthorizationHandler, HasRoleHandler>(t => new HasRoleHandler(options.ClientId));

            #endregion

            return services;
        }

        /// <summary>
        /// Agrega el midleware para el uso de la validación de Json Web Token de Keycloak 
        /// </summary>
        /// <param name="builder">Para agregar el middleware al app</param>
        /// <returns>Devuelve una referencia a la instancia después de implementar la autenticación.</returns>
        public static IApplicationBuilder UseKeycloak(this IApplicationBuilder builder)
        {
            #region PASOS 1 Y 2 (ENVIO Y REFRESCO DE TOKEN)

            if(_enableGetTokenAuto || _enableRefreshToken)
            {
                builder.UseSession();
            }

            #endregion
            #region PASOS 3 Y 4 (VALIDACION DE TOKEN)

            builder.UseAuthentication();
            
            #endregion

            return builder;
        }        
    }
}
