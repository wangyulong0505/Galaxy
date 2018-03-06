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

        /// <summary>
        /// 用户添加
        /// </summary>
        /// <returns></returns>
        public IActionResult UsersAdd()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> UsersAdd([FromBody]User entity)
        {
            try
            {
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