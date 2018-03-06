using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Galaxy.Roles;
using Galaxy.Menus;
using Galaxy.Entities;
using Abp.Web.Models;
using Newtonsoft.Json;

namespace Galaxy.Web.Controllers
{
    public class PermissionController : GalaxyControllerBase
    {
        private readonly IRoleAppService roleAppService;
        private readonly IMenuAppService menuAppService;
        public PermissionController(IRoleAppService _roleAppService, IMenuAppService _menuAppService)
        {
            roleAppService = _roleAppService;
            menuAppService = _menuAppService;
        }

        public async Task<IActionResult> Index(string strKey = "")
        {
            //获取角色列表
            List<Role> roleList = await roleAppService.GetRoles();
            if (!string.IsNullOrEmpty(strKey))
            {
                roleList = roleList.Where(q => q.Name.Contains(strKey)).ToList();
            }
            ViewData["Key"] = strKey;
            return View(roleList);
        }

        #region 获取Organization所有数据

        /// <summary>
        /// 获取所有的数据，填充TreeView
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetTreeData()
        {
            try
            {
                List<Menu> list = await menuAppService.GetMenus();
                //修改数据格式，示例数据格式如下
                /*
                var str = "[
                                {'text': '父节点 1','id':4,'tags':['44'],'nodes': 
                                    [
                                        {'text': '子节点 1','tags':['45'],'id':'2','nodes': 
                                            [
                                                {'text': '孙子节点 1','tags':['42'],'id':'3'},
                                                {'text': '孙子节点 2','tags':['41'],'id':'8'}
                                            ]
                                        },
                                        {'text': '子节点 2','tags':['45'],'id':'7'}  
                                    ]
                                }
                            ]";
                */
                string strResult = "";
                if (list != null && list.Count > 0)
                {
                    strResult = GetChildNode(list, new List<Dictionary<string, object>>());
                }
                return Json(new AjaxResponse() { Success = true, Result = strResult });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        #endregion

        #region 递归核心方法

        /// <summary>
        /// 递归调用，获取子节点信息
        /// </summary>
        /// <param name="allList">全部的列表数据</param>
        /// <param name="dicList">一个Dictionary全部存储</param>
        /// <returns></returns>
        public string GetChildNode(List<Menu> allList, List<Dictionary<string, object>> dicList)
        {
            /* 1、根据ParentNodeId==0获取所有的根节点
             * 2、遍历所有的根节点，根据根节点的Id==列表父节点Id获取所有子节点Id
             * 3、内部新建一个Dictionary，把子节点ID和Name储存在新建的Dictionary中
             * 4、递归调用传入新建的Dictionary
             * 
             * 
             */
            if (dicList == null || dicList.Count == 0)
            {
                foreach (var entity in allList)
                {
                    if (entity.ParentNodeId == 0)
                    {
                        Dictionary<string, object> rootDic = new Dictionary<string, object>();
                        rootDic.Add("id", entity.Id);
                        rootDic.Add("text", entity.Name);
                        rootDic.Add("levelCode", entity.LevelCode);
                        rootDic.Add("icon", entity.MenuIcon);
                        dicList.Add(rootDic);
                    }
                }
                GetChildNode(allList, dicList);
            }
            else if (dicList.Count > 0)
            {
                foreach (var dic in dicList)
                {
                    List<Dictionary<string, object>> sonDic = new List<Dictionary<string, object>>();
                    foreach (var son in allList)
                    {
                        string id = dic["id"].ToString();
                        string pid = son.ParentNodeId.ToString();
                        if (id.Equals(pid))
                        {
                            Dictionary<string, object> tempDic = new Dictionary<string, object>();
                            tempDic.Add("id", son.Id);
                            tempDic.Add("text", son.Name);
                            tempDic.Add("levelCode", son.LevelCode);
                            tempDic.Add("icon", son.MenuIcon);
                            sonDic.Add(tempDic);
                        }
                    }
                    if (sonDic.Count > 0)
                    {
                        List<string> nodeList = new List<string>();
                        nodeList.Add(sonDic.Count.ToString());
                        dic.Add("nodes", sonDic);
                        dic.Add("tags", nodeList);
                        GetChildNode(allList, sonDic);
                    }
                }
            }

            return JsonConvert.SerializeObject(dicList);
        }

        #endregion
    }
}