using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Attributes.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CustomerResourceAttribute : Attribute
    {
        private readonly string resourceName;

        public CustomerResourceAttribute(string _resourceName)
        {
            resourceName = _resourceName;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return resourceName; }
        }

        /// <summary>
        /// 资源描述
        /// </summary>
        public string Descript { get; set; }
    }
}
