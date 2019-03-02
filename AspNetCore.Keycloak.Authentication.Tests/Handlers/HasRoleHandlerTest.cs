using AspNetCore.KeycloakAuthentication.Handlers;
using AspNetCore.KeycloakAuthentication.Handlers.Requirements;
using AspNetCore.KeycloakAuthentication.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AspNetCore.Keycloak.Authentication.Tests.Handlers
{
    [TestClass]
    public class HasRoleHandlerTest
    {
        [TestMethod]
        public void CuandoUserNoTieneClaimsResultadoEsFail()
        {
            //
            var role = "admin";
            var audience = "TestBackend";
            var hasRole = new HasRoleHandler(audience);
            var userClaims = new ClaimsPrincipal(); //Usuario sin Claims
            var roleRequirement = new HasRoleRequirement(role);
            var requirements = new IAuthorizationRequirement[] { roleRequirement };
            var context = new AuthorizationHandlerContext(requirements, userClaims, null);

            //Act
            hasRole.ValidateRoleRequirement(context, roleRequirement);

            //Assert
            Assert.IsFalse(context.HasSucceeded);
            Assert.IsTrue(context.HasFailed);
        }

        [TestMethod]
        public void CuandoUserTieneClaimsPeroSinClaimResourceAccessResultadoEsFail()
        {
            //
            var role = "admin";
            var audience = "TestBackend";
            var hasRole = new HasRoleHandler(audience);
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "Michael Emir"));
            identity.AddClaim(new Claim(ClaimTypes.Country, "El Salvador"));

            var userClaims = new ClaimsPrincipal(identity); //Usuario con claims sin claim:resource_access
            var roleRequirement = new HasRoleRequirement(role);
            var requirements = new IAuthorizationRequirement[] { roleRequirement };
            var context = new AuthorizationHandlerContext(requirements, userClaims, null);

            //Act
            hasRole.ValidateRoleRequirement(context, roleRequirement);

            //Assert
            Assert.IsFalse(context.HasSucceeded);
            Assert.IsTrue(context.HasFailed);
        }

        [TestMethod]
        public void CuandoUserTieneClaimsPeroConClaimResourceAccessVacioResultadoEsFail()
        {
            //
            var role = "admin";
            var audience = "TestBackend";
            var hasRole = new HasRoleHandler(audience);
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "Michael Emir"));
            identity.AddClaim(new Claim(ClaimTypes.Country, "El Salvador"));
            identity.AddClaim(new Claim("resource_access", "{}"));

            var userClaims = new ClaimsPrincipal(identity); //Usuario con claims con claim:resource_access vacio
            var roleRequirement = new HasRoleRequirement(role);
            var requirements = new IAuthorizationRequirement[] { roleRequirement };
            var context = new AuthorizationHandlerContext(requirements, userClaims, null);

            //Act
            hasRole.ValidateRoleRequirement(context, roleRequirement);

            //Assert
            Assert.IsFalse(context.HasSucceeded);
            Assert.IsTrue(context.HasFailed);
        }

        [TestMethod]
        public void CuandoUserTieneClaimsPeroConClaimResourceAccessVacioResultadoEsSuccess()
        {
            //
            var role = "admin";
            var audience = "TestBackend";
            var hasRole = new HasRoleHandler(audience);
            var resourceAccess = new { TestBackend = new { roles = new string[] { role } } };

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "Michael Emir"));
            identity.AddClaim(new Claim(ClaimTypes.Country, "El Salvador"));
            identity.AddClaim(new Claim("resource_access", JsonConvert.SerializeObject(resourceAccess)));

            var userClaims = new ClaimsPrincipal(identity); //Usuario
            var roleRequirement = new HasRoleRequirement(role);
            var requirements = new IAuthorizationRequirement[] { roleRequirement };
            var context = new AuthorizationHandlerContext(requirements, userClaims, null);

            //Act
            hasRole.ValidateRoleRequirement(context, roleRequirement);

            //Assert
            Assert.IsTrue(context.HasSucceeded);
            Assert.IsFalse(context.HasFailed);
        }
    }
}