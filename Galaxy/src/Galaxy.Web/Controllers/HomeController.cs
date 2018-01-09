using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Galaxy.Web.Controllers
{
    public class HomeController : GalaxyControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// ��ȡϵͳʱ��
        /// </summary>
        /// <returns></returns>
        public JsonResult GetServerTime()
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return Json(new AjaxResponse() { Result = time });
        }
    }
}