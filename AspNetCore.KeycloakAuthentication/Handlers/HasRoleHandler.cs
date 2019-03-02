using AspNetCore.KeycloakAuthentication.Handlers.Requirements;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.KeycloakAuthentication.Handlers
{
    public class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
    {
        private static readonly string RESOURCE_CLAIM = "resource_access";
        private static readonly string ROLE_KEYWORD = "roles";

        private readonly string _clientId;

        public HasRoleHandler(string clientId)
        {
            _clientId = !string.IsNullOrEmpty(clientId) ? clientId : throw new ArgumentNullException(nameof(clientId));
        }

        public void ValidateRoleRequirement(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            var isSuccess = false;
            IEnumerable<Claim> claims = context.User?.Claims;

            if (claims != null && claims.Any())
            {
                var resourcesAccess = claims.FirstOrDefault(c => c.Type == RESOURCE_CLAIM);

                if (resourcesAccess != null)
                {
                    var resourceAccessClaimJson = JObject.Parse(resourcesAccess.Value);

                    var roles = resourceAccessClaimJson?.SelectTokens($"$.{_clientId}.{ROLE_KEYWORD}[*]")?
                                .Select(item => (string)item)?
                                .ToList();

                    if (roles != null && roles.Exists(item => item == requirement.Role))
                    {
                        context.Succeed(requirement);
                        isSuccess = true;
                    }
                }
            }

            if (!isSuccess) context.Fail();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            ValidateRoleRequirement(context, requirement);

            return Task.CompletedTask;
        }
    }
}
