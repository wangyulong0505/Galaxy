using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Utils
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public class FileHelper
    {
        #region 目录分隔符和应用程序路径前缀

        /// <summary>
        /// 目录分隔符
        /// windows "\" MacOS and Linux "/"
        /// </summary>
        public static string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();

        /// <summary>
        /// 包含应用程序目录的绝对路径
        /// </summary>
        public static string ContentRootPath = DI.ServiceProvider.GetRequiredService<IHostingEnvironment>().ContentRootPath;

        #endregion

        #region 获取绝对路径

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return IsAbsolute(path) ? path : Path.Combine(ContentRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar));
        }

        #endregion

        #region 判断是否是绝对路径

        /// <summary>
        /// 判断是否是绝对路径
        /// windows下判断是否包含 ":"
        /// Linux/Maxos下判断是否包含 "\"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAbsolute(string path)
        {
            return Path.VolumeSeparatorChar == ':' ? path.IndexOf(Path.VolumeSeparatorChar) > 0 : path.IndexOf('\\') > 0;
        }

        #endregion

        #region 检测指定路径是否存在

        public static bool IsExist(string path, bool isDirectory)
        {
            return isDirectory ? Directory.Exists(MapPath(path)) : File.Exists(MapPath(path));
        }

        #endregion

        #region 检测目录是否为空

        public static bool IsEmptyDirectory(string path)
        {
            return Directory.GetFiles(MapPath(path)).Length <= 0 && Directory.GetDirectories(MapPath(path)).Length <= 0;
        }

        #endregion

        #region 创建文件或目录

        public static void CreateFiles(string path, bool isDirectory)
        {
            try
            {
                if (!IsExist(path, isDirectory))
                {
                    if (isDirectory)
                    {
                        Directory.CreateDirectory(MapPath(path));
                    }
                    else
                    {
                        FileInfo f = new FileInfo(MapPath(path));
                        FileStream fs = f.Create();
                        fs.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 删除文件或目录

        public static void DeleteFiles(string path, bool isDirectory)
        {
            try
            {
                if (IsExist(path, isDirectory))
                {
                    if (isDirectory)
                    {
                        Directory.Delete(MapPath(path));
                    }
                    else
                    {
                        File.Delete(MapPath(path));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 清除目录下所有文件和子目录，保留该目录

        public static void ClearDirectory(string path)
        {
            try
            {
                if (IsExist(path, true))
                {
                    //目录下的所有文件
                    string[] files = Directory.GetFiles(MapPath(path));
                    foreach (string file in files)
                    {
                        DeleteFiles(file, false);
                    }

                    //目录下的所有子目录
                    string[] directorys = Directory.GetDirectories(MapPath(path));
                    foreach (string dir in directorys)
                    {
                        DeleteFiles(dir, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 复制移动文件

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="isOverWrite"></param>
        public static void Copy(string sourcePath, string targetPath, bool isOverWrite = true)
        {
            File.Copy(MapPath(sourcePath), MapPath(targetPath), isOverWrite);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        public static void Move(string sourcePath, string targetPath)
        {
            string sourceFileName = GetFileName(sourcePath);

            //如果目录不存在就创建
            if (!IsExist(targetPath, true))
            {
                CreateFiles(targetPath, true);
            }
            else
            {
                //如果目标目录存在同名文件就删除
                if (IsExist(Path.Combine(MapPath(targetPath), sourceFileName), false))
                {
                    DeleteFiles(Path.Combine(MapPath(targetPath)), true);
                }
            }

            File.Move(MapPath(sourcePath), MapPath(targetPath));
        }

        #endregion

        #region 获取文件名和扩展名

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(MapPath(path));
        }

        /// <summary>
        /// 获取文件名不带扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(MapPath(path));
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            return Path.GetExtension(MapPath(path));
        }

        #endregion
    }
}
