using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.KeycloakAuthentication.Handlers.Requirements
{
    public class HasRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public HasRoleRequirement(string role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}
