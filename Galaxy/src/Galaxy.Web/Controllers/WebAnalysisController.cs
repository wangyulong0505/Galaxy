using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy.Web.Controllers
{
    /// <summary>
    /// 网站分析--主要用到百度统计插件
    /// </summary>
    public class WebAnalysisController : GalaxyControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}