using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy.Web.Controllers
{
    public class UsersController : GalaxyControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户添加
        /// </summary>
        /// <returns></returns>
        public IActionResult UsersAdd()
        {
            return View();
        }



        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult UsersEdit(int Id)
        {
            return View();
        }
    }
}