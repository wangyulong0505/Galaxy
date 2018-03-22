using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Galaxy.Web.Attributes
{
    public class ResourceAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions options;
        public ResourceAuthorizationPolicyProvider(IOptions<AuthorizationOptions> _options)
        {
            options = _options.Value;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(options.DefaultPolicy);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy policy = options.GetPolicy(policyName);
            if (policy == null)
            {
                string[] resourceValues = policyName.Split(new char[] { '-' }, StringSplitOptions.None);
                if (resourceValues.Length == 1)
                {
                    options.AddPolicy(policyName, builder =>
                    {
                        builder.AddRequirements(new ClaimsAuthorizationRequirement(resourceValues[0], null));
                    });
                }
                else
                {
                    options.AddPolicy(policyName, builder =>
                    {
                        builder.AddRequirements(new ClaimsAuthorizationRequirement(resourceValues[0], new string[] { resourceValues[1] }));
                    });
                }
            }
            return Task.FromResult(options.GetPolicy(policyName));
        }
    }
}
