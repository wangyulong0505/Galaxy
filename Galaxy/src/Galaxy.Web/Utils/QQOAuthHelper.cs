using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Web.Utils
{
    public class QQOAuthHelper
    {
        private readonly string appId;
        private readonly string appKey;
        private readonly string qqCallBack;
        public QQOAuthHelper(string strAppID, string strAppKey, string strQQCallBack)
        {
            appId = strAppID;
            appKey = strAppKey;
            qqCallBack = strQQCallBack;
        }

        /// <summary>
        /// 获取oauth信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public QQOauthInfo GetOauthInfo(string code)
        {
            string callback = System.Web.HttpUtility.UrlEncode(qqCallBack, Encoding.UTF8);
            string url = string.Format("https://graph.qq.com/oauth2.0/token?grant_type={0}&client_id={1}&client_secret={2}&code={3}&redirect_uri={4}", "authorization_code", appId, appKey, code, callback);
            string res = LoadHtmlUserGetType(url, Encoding.UTF8);
            QQOauthInfo qqOauthInfo = new QQOauthInfo();
            qqOauthInfo.AccessToken = CutString(res, "access_token=", "&expires_in=");
            qqOauthInfo.ExpiresIn = CutString(res, "&expires_in=", "&refresh_token=");
            qqOauthInfo.RefreshToken = res.Split(new string[] { "&refresh_token=" }, StringSplitOptions.None)[1];
            return qqOauthInfo;
        }
        /// <summary>
        /// 截取字符串中两个字符串中的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startStr">开始字符串</param>
        /// <param name="endStr">结束字符串</param>
        /// <returns></returns>
        private string CutString(string str, string startStr, string endStr)
        {
            int begin, end;
            begin = str.IndexOf(startStr, 0) + startStr.Length; //开始位置
            end = str.IndexOf(endStr, begin); //结束位置
            return str.Substring(begin, end - begin); //取搜索的条数，用结束的位置-开始的位置,并返回
        }
        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public string LoadHtmlUserGetType(string urlString, Encoding encoding)
        {

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebRespones = null;
            Stream stream = null;
            string htmlString = string.Empty;
            try
            {
                httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
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
        /// 获取QQ账号的OpenID
        /// </summary>
        /// <param name="qqOauthInfo"></param>
        /// <returns></returns>
        public string GetOpenID(QQOauthInfo qqOauthInfo)
        {
            string res = LoadHtmlUserGetType("https://graph.qq.com/oauth2.0/me?access_token=" + qqOauthInfo.AccessToken, Encoding.UTF8);
            return CutString(res, @"openid"":""", @"""}");
        }
        /// <summary>
        /// 获取QQ昵称
        /// </summary>
        /// <param name="qqOauthInfo"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public string GetUserInfo(QQOauthInfo qqOauthInfo, string openID)
        {

            string urlGetInfo = string.Format(@"https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}", qqOauthInfo.AccessToken, appId, openID);
            string resUserInfo = LoadHtmlUserGetType(urlGetInfo, Encoding.UTF8);
            return CutString(resUserInfo, @"""nickname"": """, @""",");
        }
    }
    public class QQOauthInfo
    {
        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
