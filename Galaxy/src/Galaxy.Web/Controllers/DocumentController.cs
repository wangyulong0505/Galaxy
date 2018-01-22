using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Galaxy.Documents;
using Galaxy.Entities;
using Galaxy.Web.Utils;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Abp.Web.Models;
using Newtonsoft.Json;
using Galaxy.Web.Models.Document;

namespace Galaxy.Web.Controllers
{
    public class DocumentController : GalaxyControllerBase
    {
        private readonly IDocumentAppService documentAppService;
        private readonly IHostingEnvironment hostingEnvironment;
        public DocumentController(IHostingEnvironment _hostingEnvironment, IDocumentAppService _documentAppService)
        {
            hostingEnvironment = _hostingEnvironment;
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
                ViewData["Id"] = 0;
            }
            else
            {
                //修改操作，根据Id获取数据
                ViewData["Id"] = Id;
            }
            return View();
        }

        /// <summary>
        /// 保存编辑的内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task<JsonResult> MarkdownSave([FromBody]MarkdownViewModel entity)
        {
            Document doc = new Document();
            //Document entity = JsonConvert.DeserializeObject<Document>(str);
            try
            {
                if (entity.Id == 0)
                {
                    doc.KeyWords = entity.KeyWords;
                    doc.Content = entity.Content;
                    doc.CreateTime = DateTime.Now;
                    doc.Status = 0;
                    doc.Title = entity.Title;
                    await documentAppService.PostDocument(doc);
                }
                else
                {
                    doc.Id = entity.Id;
                    doc.KeyWords = entity.KeyWords;
                    doc.Content = entity.Content;
                    doc.CreateTime = DateTime.Now;
                    doc.Status = 0;
                    doc.Title = entity.Title;
                    await documentAppService.PutDocument(doc);
                }

                return Json(new AjaxResponse { Success=true, Result="" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        /// <summary>
        /// 预览界面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Preview(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View();
            }
            Document entity = await documentAppService.GetDocumentDetail(Convert.ToInt32(Id));

            return View(entity);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public JsonResult MarkdownUpload(IFormFile strFile)
        {
            //MVC可以使用HttpPostedFileBase， NetCore下需要使用IFormFile
            long size = 0;
            //获取文件名
            string strFileName = ContentDispositionHeaderValue.Parse(strFile.ContentDisposition).FileName.Trim();
            string strExt = strFileName.Substring(strFileName.LastIndexOf('.')).Replace("\"", "");
            //自定义新文件名
            string strNewName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{strExt}";
            //合成路径
            string strFilePath = hostingEnvironment.WebRootPath + @"\File\Upload\Markdown\" + strNewName;
            size += strFile.Length;
            using (FileStream fs = System.IO.File.Create(strFilePath))
            {
                strFile.CopyTo(fs);
                fs.Flush();
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("success", "1");
            dic.Add("message", "");
            dic.Add("url", strFilePath);
            return Json(new AjaxResponse { Result = JsonConvert.SerializeObject(dic) });
        }

        /// <summary>
        /// 根据Id获取实体数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> Markdown(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return Json(new AjaxResponse { Success = false, Error = new ErrorInfo("Id为空"), Result = "" });
            }
            Document entity = await documentAppService.GetDocumentDetail(Convert.ToInt32(Id));

            return Json(new AjaxResponse { Result = JsonConvert.SerializeObject(entity) });
        }
    }
}