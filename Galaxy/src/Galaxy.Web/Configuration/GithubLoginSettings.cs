using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Configuration
{
    public class GithubLoginSettings
    {
        /// <summary>
        /// GithubAppId
        /// </summary>
        public string GithubAppID { get; set; }

        /// <summary>
        /// GithubAppKey
        /// </summary>
        public string GithubAppKey { get; set; }

        /// <summary>
        /// Github回调地址
        /// </summary>
        public string GithubCallBack { get; set; }

        /// <summary>
        /// Github认证Url
        /// </summary>
        public string GithubAuthorizeURL { get; set; }
    }
}
