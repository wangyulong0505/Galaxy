using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy.Web.Controllers
{
    public class DocumentController : GalaxyControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 编辑界面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit(int Id = 0)
        {
            //根据Id是否为0判断是新增还是修改
            if (Id == 0)
            {
                //新增操作，直接返回
            }
            else
            {
                //修改操作，根据Id获取数据
            }
            return View();
        }

        /// <summary>
        /// 预览界面
        /// </summary>
        /// <returns></returns>
        public IActionResult Preview()
        {
            return View();
        }
    }
}