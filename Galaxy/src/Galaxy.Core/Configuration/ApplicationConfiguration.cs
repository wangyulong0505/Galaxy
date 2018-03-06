using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Configuration
{
    /// <summary>
    /// 配置文件类，对应Web项目下的Json配置文件，例如：appsettings.json， siteconfig.json
    /// </summary>
    public class ApplicationConfiguration
    {
        #region SiteBaseConfig属性字段配置

        public string FileUpPath { get; set; }

        public bool IsSingleLogin { get; set; }

        public string AttachExtension { get; set; }

        public int AttachImageSize { get; set; }

        #endregion

        #region RedisCacheConfig属性字段配置

        public bool IsUseRedis { get; set; }

        public string ConnectionString { get; set; }

        public string InstanceName { get; set; }

        #endregion
    }
}
