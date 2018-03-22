using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ResourceAttribute: AuthorizeAttribute
    {
        private string resourceName;
        private string action;
        public ResourceAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            resourceName = name;
            Policy = resourceName;
        }

        public string GetResource()
        {
            return resourceName;
        }

        public string Action
        {
            get { return action; }
            set
            {
                action = value;
                if (string.IsNullOrEmpty(value))
                {
                    //把资源名称跟操作名称组装成Policy
                    Policy = resourceName + "-" + value;
                }
            }
        }
    }
}
