using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Configuration
{
    public class WeiboLoginSettings
    {
        /// <summary>
        /// 微博AppId
        /// </summary>
        public string WeiboAppID { get; set; }

        /// <summary>
        /// 微博AppKey
        /// </summary>
        public string WeiboAppKey { get; set; }

        /// <summary>
        /// 微博回调地址
        /// </summary>
        public string WeiboCallBack { get; set; }

        /// <summary>
        /// 微博认证Url
        /// </summary>
        public string WeiboAuthorizeURL { get; set; }
    }
}
