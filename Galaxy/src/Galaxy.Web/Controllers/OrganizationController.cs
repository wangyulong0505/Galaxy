using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Galaxy.Organization;
using Newtonsoft.Json;

namespace Galaxy.Web.Controllers
{
    public class OrganizationController : GalaxyControllerBase
    {
        private readonly IOrganizationAppService orgAppService;
        public OrganizationController(IOrganizationAppService _orgAppService)
        {
            orgAppService = _orgAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有的数据，填充TreeView
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTreeData()
        {
            List<Entities.Organization> list = orgAppService.GetOrganizationList();
            return Json(new AjaxResponse() { Result = JsonConvert.SerializeObject(list)});
        }

        /// <summary>
        /// 根据Id获取单条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetNodeData(int Id)
        {
            Entities.Organization entity = orgAppService.GetOrganizationById(Id);
            return Json(new AjaxResponse() { Result = JsonConvert.SerializeObject(entity) });
        }
    }
}