using AspNetCore.KeycloakAuthentication.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Keycloak.Authentication.Tests.Configuration
{
    [TestClass]
    public class KeycloakAuthenticationOptionsTest
    {
        [TestMethod]
        public void CuandoUrlTienePlecaAlFinal()
        {
            var uri = "http://artemis:8085/auth/";
            var options = new KeycloakOptions { KeycloakUrl = uri, Realm = "prueba" };

            var result = options.KeycloakUrlRealm;

            Assert.AreEqual("http://artemis:8085/auth/realms/prueba", result);
        }

        [TestMethod]
        public void CuandoUrlTieneSinPlecaAlFinal()
        {
            var uri = "http://artemis:8085/auth";
            var options = new KeycloakOptions { KeycloakUrl = uri, Realm = "prueba" };

            var result = options.KeycloakUrlRealm;

            Assert.AreEqual("http://artemis:8085/auth/realms/prueba", result);
        }
    }
}
