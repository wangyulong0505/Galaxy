using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Galaxy.Messages;
using Galaxy.Entities;
using Galaxy.Web.Utils;
using Abp.Web.Models;

namespace Galaxy.Web.Controllers
{
    public class MessageController : GalaxyControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IMessageAppService messageAppService;
        public MessageController(IHostingEnvironment _hostingEnvironment, IMessageAppService _messageAppService)
        {
            hostingEnvironment = _hostingEnvironment;
            messageAppService = _messageAppService;
        }

        /// <summary>
        /// 默认是收件箱 0收件箱， 1发件箱， 2草稿箱， 3回收站
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int pageIndex = 1, int pageSize = 10, string strKey = "")
        {
            List<Message> inboxList = messageAppService.GetPagingMessage(2, pageIndex, pageSize, strKey, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = strKey;
            return View(inboxList);
        }

        /// <summary>
        /// 发件箱 0收件箱， 1发件箱， 2草稿箱， 3回收站
        /// </summary>
        /// <returns></returns>
        public IActionResult Outbox(int pageIndex = 1, int pageSize = 10, string strKey = "")
        {
            List<Message> outboxList = messageAppService.GetPagingMessage(1, pageIndex, pageSize, strKey, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = strKey;
            return View(outboxList);
        }

        /// <summary>
        /// 草稿箱 0收件箱， 1发件箱， 2草稿箱， 3回收站
        /// </summary>
        /// <returns></returns>
        public IActionResult Draft(int pageIndex = 1, int pageSize = 10, string strKey = "")
        {
            List<Message> draftList = messageAppService.GetPagingMessage(0, pageIndex, pageSize, strKey, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = strKey;
            return View(draftList);
        }

        /// <summary>
        /// 垃圾箱 0收件箱， 1发件箱， 2草稿箱， 3回收站
        /// </summary>
        /// <returns></returns>
        public IActionResult Trash(int pageIndex = 1, int pageSize = 10, string strKey = "")
        {
            List<Message> trashList = messageAppService.GetPagingMessage(3, pageIndex, pageSize, strKey, out int pageCount, out int itemCount);
            ViewBag.Page = TablePagination.PagingHtml(pageIndex, pageSize, pageCount, itemCount, strKey);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.Key = strKey;
            return View(trashList);
        }

        /// <summary>
        /// 消息新增页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Add([FromBody]Message entity)
        {
            try
            {
                //
                return Json(new AjaxResponse { Success = true, Result = "" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new AjaxResponse { Success = false, Result = ex.Message });
            }
        }

        /// <summary>
        /// 消息预览页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Preview()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FileSave()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string rootPath = hostingEnvironment.WebRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string strFileExt = GetFileExt(formFile.FileName);
                    long FileSize = formFile.Length;
                    string strNewFileName = System.Guid.NewGuid().ToString() + "." + strFileExt;
                    string strFilePath = rootPath + "/Upload/" + strNewFileName;
                    using (System.IO.FileStream stream = new System.IO.FileStream(strFilePath, System.IO.FileMode.OpenOrCreate))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }

        /// <summary>
        /// 文件流的方式输出
        /// </summary>
        /// <param name="file">文件的绝对路径带扩展名</param>
        /// <returns></returns>
        public IActionResult DownloadFile(string file)
        {
            string addUrl = file;
            System.IO.FileStream stream = System.IO.File.OpenRead(addUrl);
            //因Path.GetExtension带'.', 所以使用自定义的方法
            string strFileExt = GetFileExt(file);
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            string contentType = provider.Mappings[strFileExt];
            return File(stream, contentType, System.IO.Path.GetFileName(addUrl));
        }

        /// <summary>
        /// 获取文件扩展名，不带'.'
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetFileExt(string file)
        {
            string ext = file.Substring(file.LastIndexOf('.') + 1, file.Length);
            return ext;
        }
    }
}