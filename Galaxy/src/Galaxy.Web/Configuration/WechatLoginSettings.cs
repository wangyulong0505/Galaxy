using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Configuration
{
    public class WechatLoginSettings
    {
        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WechatAppID { get; set; }

        /// <summary>
        /// 微信AppKey
        /// </summary>
        public string WechatAppKey { get; set; }

        /// <summary>
        /// 微信回调地址
        /// </summary>
        public string WechatCallBack { get; set; }

        /// <summary>
        /// 微信认证Url
        /// </summary>
        public string WechatAuthorizeURL { get; set; }
    }
}
