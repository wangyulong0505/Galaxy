using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Galaxy.Roles;
using Galaxy.Users;
using Galaxy.Entities;
using Galaxy.Web.Utils;
using Abp.Web.Models;
using Newtonsoft.Json;

namespace Galaxy.Web.Controllers
{
    public class RolesController : GalaxyControllerBase
    {
        private readonly IRoleAppService roleService;
        private readonly IUserAppService userService;
        public RolesController(IRoleAppService _roleService, IUserAppService _userService)
        {
            roleService = _roleService;
            userService = _userService;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string strRoleKey = "", string strUserKey = "")
        {
            List<Role> roleList = await roleService.GetRoles();
            //获取所有的Role和分页获取Users
            if (string.IsNullOrEmpty(strRoleKey))
            {
                roleList = roleList.Where(q => q.Name.Contains(strRoleKey)).ToList();
            }
            List<User> userList = userService.GetPagingUsers(pageIndex, pageSize, strUserKey, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strUserKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.UserKey = strUserKey;
            ViewBag.RoleKey = strRoleKey;
            ViewData["RoleList"] = roleList;
            ViewData["UserList"] = userList;
            return View();
        }

        public async Task<JsonResult> RoleEdit(int Id = 0)
        {
            try
            {
                //Id==0是新增的, 否则是修改的
                if (Id == 0)
                {
                    return Json(new AjaxResponse { Success = true, Result = "" });
                }
                else
                {
                    Role role = await roleService.GetRoleDetail(Id);

                    return Json(new AjaxResponse { Success = true, Result = JsonConvert.SerializeObject(role) });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> RoleSave(Role entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    await roleService.PostRole(entity);
                }
                else
                {
                    await roleService.PutRole(entity);
                }

                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }
    }
}