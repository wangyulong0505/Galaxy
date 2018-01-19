using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authorization;

namespace Galaxy.Web.Filters
{
    /// <summary>
    /// 位SwaggerUI添加http头部信息，如权限认证信息
    /// </summary>
    public class HttpHeaderOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //先判断参数是否为空，如果为空先初始化
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            //获取Api的描述，action特性，判断是否含有AuthorizeAttribute标记，如果没有判断controller是否含有
            var actionAttr = context.ApiDescription.ActionAttributes();
            var isAuthorize = actionAttr.Any(a => a.GetType() == typeof(AuthorizeAttribute));
            if (!isAuthorize)
            {
                var controllerAttr = context.ApiDescription.ControllerAttributes();
                isAuthorize = controllerAttr.Any(a => a.GetType() == typeof(AuthorizeAttribute));
            }
            //判断action是否含有AllowAnonymousAttribute特性标记
            var isAllowAnonymous = actionAttr.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
            if (isAuthorize && isAllowAnonymous == false)
            {
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "string",
                    Required = false
                });
            }
        }
    }
}
