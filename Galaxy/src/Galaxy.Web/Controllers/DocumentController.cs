using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Galaxy.Documents;
using Galaxy.Entities;
using Galaxy.Web.Utils;

namespace Galaxy.Web.Controllers
{
    public class DocumentController : GalaxyControllerBase
    {
        private readonly IDocumentAppService documentAppService;
        public DocumentController(IDocumentAppService _documentAppService)
        {
            documentAppService = _documentAppService;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 10, string strKey = "")
        {
            
            List<Document> docList = documentAppService.GetPagingDocuments(pageIndex, pageSize, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = strKey;
            return View(docList);
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