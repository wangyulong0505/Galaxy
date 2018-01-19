using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Galaxy.Web.Controllers
{
    /// <summary>
    /// 网站分析--主要用到百度统计插件
    /// </summary>
    public class WebAnalysisController : GalaxyControllerBase
    {
        //private readonly IList<ApiRequest>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取百度访客区域统计数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetVisitDistrictAnalytics(string startDate, string endDate)
        {
            HttpClient hc = new HttpClient();
            //调用百度的Api接口获取数据
            string url = "https://api.baidu.com/json/tongji/v1/ReportService/getSiteList";
            //var data = 
            HttpContent data = null;
            return Json(await hc.PostAsync(url, data));
        }

        /// <summary>
        /// 获取百度趋势分析数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetTrendAnalytics(string startDate, string endDate)
        {
            HttpClient client = new HttpClient();
            //调用百度的Api接口获取数据
            string url = "https://api.baidu.com/json/tongji/v1/ReportService/getData";
            //var data = 
            HttpContent data = null;
            return Json(await client.PostAsync(url, data));
        }
    }
}