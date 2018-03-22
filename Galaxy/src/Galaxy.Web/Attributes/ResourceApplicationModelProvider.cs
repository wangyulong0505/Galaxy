using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Attributes
{
    public class ResourceApplicationModelProvider : IApplicationModelProvider
    {
        private readonly IAuthorizationPolicyProvider policyProvider;
        public ResourceApplicationModelProvider(IAuthorizationPolicyProvider _provider)
        {
            policyProvider = _provider;
        }

        public int Order { get { return -1000 + 11; } }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            throw new NotImplementedException();
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            List<ResourceAttribute> attributeData = new List<ResourceAttribute>();
            foreach (var controlModel in context.Result.Controllers)
            {
                //得到方法的ResourceAttribute
                var resourceData = controlModel.Attributes.OfType<ResourceAttribute>().ToArray();
                if (resourceData.Length > 0)
                {
                    attributeData.AddRange(resourceData);
                }
                //循环控制器方法
                foreach (var actionModel in controlModel.Actions)
                {
                    //得到方法的ResourceAttribute
                    var actionResourceData = actionModel.Attributes.OfType<ResourceAttribute>().ToArray();
                    if (actionResourceData.Length > 0)
                    {
                        attributeData.AddRange(actionResourceData);
                    }
                }
            }
            //把所有的resourceattribute的信息写到一个全局的resourcedata中，resourcedata就可以在其他地方进行使用，resourcedata定义后面补充　
            foreach (var item in attributeData)
            {
                ResourceData.AddResource(item.GetResource(), item.Action);
            }
        }
    }
}
