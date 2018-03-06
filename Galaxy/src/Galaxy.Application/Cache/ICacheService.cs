using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Cache
{
    /// <summary>
    /// 缓存服务接口
    /// </summary>
    public interface ICacheService
    {
        #region 验证缓存是否存在

        bool Exists(string strKey);

        Task<bool> ExistsAsync(string strKey);

        #endregion

        #region 添加缓存

        bool Add(string strKey, object value);

        Task<bool> AddAsync(string strKey, object value);

        bool Add(string strKey, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte);

        Task<bool> AddAsync(string strKey, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte);

        bool Add(string strKey, object value, TimeSpan expiresIn, bool isSliding = false);

        Task<bool> AddAsync(string strKey, object value, TimeSpan expiresIn, bool isSliding = false);

        #endregion

        #region 删除缓存

        bool Remove(string strKey);

        Task<bool> RemoveAsync(string strKey);

        void RemoveAll(IEnumerable<string> keys);

        Task RemoveAllAsync(IEnumerable<string> keys);

        #endregion

        #region 获取缓存

        T Get<T>(string strKey) where T : class;

        Task<T> GetAsync<T>(string strKey) where T : class;

        object Get(string strKey);

        Task<object> GetAsync(string strKey);

        IDictionary<string, object> GetAll(IEnumerable<string> keys);

        Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys);

        #endregion

        #region 修改缓存

        bool Replace(string key, object value);

        Task<bool> ReplaceAsync(string key, object value);

        bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte);

        Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte);

        bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false);

        Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false);

        #endregion
    }
}
