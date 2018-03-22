using Abp.Web.Models;
using Galaxy.Entities;
using Galaxy.Users;
using Galaxy.Organizations;
using Galaxy.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Galaxy.Web.Attributes;

namespace Galaxy.Web.Controllers
{
    public class UsersController : GalaxyControllerBase
    {
        private readonly IUserAppService userAppService;
        private readonly IOrganizationAppService orgAppService;
        public UsersController(IUserAppService _userAppService, IOrganizationAppService _orgAppService)
        {
            userAppService = _userAppService;
            orgAppService = _orgAppService;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 10, string strKey = "")
        {
            List<User> userList = userAppService.GetPagingUsers(pageIndex, pageSize, strKey, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = strKey;
            return View(userList);
        }

        [Resource("用户管理", Action = "添加用户")]
        public IActionResult UsersAdd()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> UsersAdd([FromBody]User entity)
        {
            /*
             * erp系统用户密码问题有两种
             * 1、新增用户的时候提供一个输入框供输入密码，
             * 2、新增用户时不提供密码输入框，随机生成一个密码，用户自己登录后修改密码
             * 这两种方法各有所长，由于一般添加用户的不是用户本人，所以这里使用第二种方法
             * 密码使用的是MD5加密生成32位字节，初始设定密码和用户名一样
             */
            try
            {
                entity.Password = Core.CommonFuns.Encrypt.Md5(entity.UserName);
                await userAppService.PostUser(entity);
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> UsersEdit(int Id)
        {
            User entity = await userAppService.GetUserDetail(Id);
            if (entity.DepartmentId == 0)
            {
                ViewData["DepartmentName"] = "";
            }
            else
            {
                Organization org = await orgAppService.GetOrganization(entity.DepartmentId);
                ViewData["DepartmentName"] = org.Name;
            }
            return View(entity);
        }

        /// <summary>
        /// 用户编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> UsersEdit([FromBody]User entity)
        {
            try
            {
                await userAppService.PutUser(entity);
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UsersDelete(int Id)
        {
            try
            {
                await userAppService.DeleteUser(Id);
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <returns></returns>
        public JsonResult Avatar(IFormFile strFiles)
        {
            return Json(new AjaxResponse { });
        }
    }
}