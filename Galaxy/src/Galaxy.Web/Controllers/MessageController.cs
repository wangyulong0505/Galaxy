using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Galaxy.Web.Controllers
{
    public class MessageController : GalaxyControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public MessageController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 消息新增页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
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