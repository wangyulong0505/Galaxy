using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Web.Utils
{
    public class WeiboOAuthHelper
    {
        private readonly string AppId;
        private readonly string AppKey;
        private readonly string WeiboCallBack;

        public WeiboOAuthHelper(string strAppId, string strAppKey, string strWeiboCallBack)
        {
            AppId = strAppId;
            AppKey = strAppKey;
            WeiboCallBack = strWeiboCallBack;
        }
        /// <summary>
        /// 获取oauth信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WeiboOauthInfo GetOauthInfo(string code)
        {
            string callback = System.Web.HttpUtility.UrlEncode(WeiboCallBack, Encoding.UTF8);
            string url = string.Format("https://api.weibo.com/oauth2/access_token?grant_type={0}&client_id={1}&client_secret={2}&code={3}&redirect_uri={4}", "authorization_code", AppId, AppKey, code, callback);
            string res = LoadHtmlUserGetType(url, Encoding.UTF8, "POST");
            WeiboOauthInfo OauthInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiboOauthInfo>(res);
            return OauthInfo;
        }

        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public string LoadHtmlUserGetType(string urlString, Encoding encoding, string method)
        {

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebRespones = null;
            Stream stream = null;
            string htmlString = string.Empty;
            try
            {
                httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
                httpWebRequest.Method = method;
            }
            catch (Exception ex)
            {
                throw new Exception("建立页面请求时发生错误！", ex);
            }

            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
            try
            {
                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebRespones.GetResponseStream();
            }
            catch (Exception ex)
            {
                throw new Exception("接受服务器返回页面时发生错误！", ex);
            }
            StreamReader streamReader = new StreamReader(stream, encoding);
            try
            {
                htmlString = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("读取页面数据时发生错误！", ex);
            }
            streamReader.Close();
            stream.Close();
            return htmlString;
        }
        /// <summary>
        /// 获取微博账号的OpenID
        /// </summary>
        /// <param name="qqOauthInfo"></param>
        /// <returns></returns>
        public string GetOpenID(WeiboOauthInfo oauthInfo)
        {
            string res = LoadHtmlUserGetType("https://api.weibo.com/2/account/get_uid.json?access_token=" + oauthInfo.Access_token, Encoding.UTF8, "GET");
            WeiboUserID userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiboUserID>(res);
            return userInfo.Uid;
        }
        /// <summary>
        /// 获取微博昵称
        /// </summary>
        /// <param name="qqOauthInfo"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public string GetUserInfo(WeiboOauthInfo WeiboOauthInfo, string userID)
        {

            string urlGetInfo = string.Format(@"https://api.weibo.com/2/users/show.json?access_token={0}&uid={1}", WeiboOauthInfo.Access_token, userID);
            string jsonUserInfo = LoadHtmlUserGetType(urlGetInfo, Encoding.UTF8, "GET");
            WeiboFullUserInfo fullUserInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiboFullUserInfo>(jsonUserInfo);
            return fullUserInfo.Screen_name;
        }
    }
    public class WeiboOauthInfo
    {
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
        public string Remind_in { get; set; }
        public string Uid { get; set; }
    }
    public class WeiboUserID
    {
        public string Uid { get; set; }

    }
    public class WeiboFullUserInfo
    {
        public Int64 Id { get; set; }
        public string Screen_name { get; set; }
    }
}
