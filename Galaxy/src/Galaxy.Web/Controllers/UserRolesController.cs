using Abp.Web.Models;
using Galaxy.Entities;
using Galaxy.Roles;
using Galaxy.UserRoles;
using Galaxy.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Controllers
{
    public class UserRolesController : GalaxyControllerBase
    {
        private readonly IRoleAppService roleService;
        private readonly IUserAppService userService;
        private readonly IUserRoleAppService userRoleAppService;
        public UserRolesController(IRoleAppService _roleService, IUserAppService _userService, IUserRoleAppService _userRoleAppService)
        {
            roleService = _roleService;
            userService = _userService;
            userRoleAppService = _userRoleAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">RoleId</param>
        /// <returns></returns>
        public IActionResult Index(int Id)
        {
            ViewData["RoleId"] = Id;
            return View();
        }

        public async Task<JsonResult> GetUnbindList(int RoleId, int offset, string name)
        {
            List<User> unbindList = await userRoleAppService.GetExcludeUsersByRoleId(RoleId);
            int total = unbindList.Count;
            List<User> rows = unbindList.Skip(offset).Take(10).ToList();
            return Json(new { total = total, rows = rows });
        }

        public async Task<JsonResult> GetBindList(int RoleId, int offset, string name)
        {
            List<User> bindList = await userRoleAppService.GetUsersByRoleId(RoleId);
            int total = bindList.Count;
            List<User> rows = bindList.Skip(offset).Take(10).ToList();
            return Json(new { total = total, rows = rows });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> AddUserRoles([FromBody]UserRole entity)
        {
            try
            {
                await userRoleAppService.PostUserRole(entity);
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> RemoveUserRoles([FromBody]UserRole entity)
        {
            try
            {
                await userRoleAppService.RemoveUserRole(entity);
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