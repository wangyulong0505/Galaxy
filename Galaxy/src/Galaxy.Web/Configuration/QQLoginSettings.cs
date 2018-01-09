using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Configuration
{
    public class QQLoginSettings
    {
        /// <summary>
        /// AppID
        /// </summary>
        public string QQAppID { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        public string QQAppKey { get; set; }

        /// <summary>
        /// 回调链接
        /// </summary>
        public string QQCallBack { get; set; }

        /// <summary>
        /// QQ认证链接
        /// </summary>
        public string QQAuthorizeURL { get; set; }
    }
}
