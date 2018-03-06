using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Configuration
{
    public class RedisCacheConfig
    {
        #region RedisCacheConfig属性字段配置

        public bool IsUseRedis { get; set; }

        public string ConnectionString { get; set; }

        public string InstanceName { get; set; }

        #endregion
    }
}
