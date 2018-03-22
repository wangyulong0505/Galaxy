using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Attributes.Requirement
{
    public class MyRequirement : IAuthorizationRequirement
    {
        public string Name { get; set; }
    }
}
