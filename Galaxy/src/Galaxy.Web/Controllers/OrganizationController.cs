using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Galaxy.Organizations;
using Newtonsoft.Json;
using Galaxy.Entities;

namespace Galaxy.Web.Controllers
{
    public class OrganizationController : GalaxyControllerBase
    {
        #region 对象初始化和依赖注入

        private readonly IOrganizationAppService orgAppService;
        public OrganizationController(IOrganizationAppService _orgAppService)
        {
            orgAppService = _orgAppService;
        }

        #endregion

        #region 首页

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region 获取全部数据初始化树菜单

        #region 获取Organization所有数据

        /// <summary>
        /// 获取所有的数据，填充TreeView
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetTreeData()
        {
            try
            {
                List<Organization> list = await orgAppService.GetOrganizations();
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
                return Json(new AjaxResponse() { Success = true, Result =  strResult });
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
        public string GetChildNode(List<Organization> allList, List<Dictionary<string, object>> dicList)
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

        #region 根据Id获取单条记录

        /// <summary>
        /// 根据Id获取单条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetNodeData(int Id)
        {
            try
            {
                Organization entity = await orgAppService.GetOrganization(Id);
                return Json(new AjaxResponse() { Result = JsonConvert.SerializeObject(entity) });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        #endregion

        #endregion

        #region 删除节点

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteNode(int Id)
        {
            try
            {
                await orgAppService.DeleteOrganization(Id);
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        #endregion

        #region 保存修改

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> SaveChange([FromBody]Organization entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    await orgAppService.PostOrganization(entity);
                }
                else
                {
                    await orgAppService.PutOrganization(entity);
                }
                
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        #endregion

        #region 检查编码是否唯一

        /// <summary>
        /// 检查编码是否唯一
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual async Task<JsonResult> CheckCodeUnique(string code)
        {
            bool result = true;
            List<Organization> orgList = await orgAppService.GetOrganizations();
            foreach (Organization org in orgList)
            {
                if (org.Code.Equals(code))
                {
                    result = false;
                    break;
                }
            }
            Dictionary<string, bool> dic = new Dictionary<string, bool>
            {
                { "valid", result }
            };
            //转化为Json输出
            //返回数据
            return Json(JsonConvert.SerializeObject(dic));
        }

        #endregion
    }
}