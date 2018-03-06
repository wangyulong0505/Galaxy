using Galaxy.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Views.Shared.Components.Message
{
    [ViewComponent(Name = "Message")]
    public class MessageViewComponent : ViewComponent
    {
        private readonly IMessageAppService messageAppService;
        public MessageViewComponent(IMessageAppService _messageAppService)
        {
            messageAppService = _messageAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //由于一个用户可以有多个角色，所以需要根据用户Id获取所有角色的权限，然后求合集就是用户的所有权限
            var userId = HttpContext.Session.GetString("Id");
            List<Entities.Message> messageList = await messageAppService.GetMessages();

            return View();
        }
    }
}
