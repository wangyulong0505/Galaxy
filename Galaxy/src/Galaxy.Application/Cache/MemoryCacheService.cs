using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Cache
{
    /// <summary>
    /// 缓存操作实现类MemoryCacheService
    /// </summary>
    public class MemoryCacheService : ICacheService
    {

        private readonly IMemoryCache cache;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_cache"></param>
        public MemoryCacheService(IMemoryCache _cache)
        {
            cache = _cache;
        }

        #region 验证缓存是否存在

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            object cached;
            return cache.TryGetValue(key, out cached);
        }

        /// <summary>
        /// 验证缓存项是否存在（异步）
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string strKey)
        {
            if (string.IsNullOrEmpty(strKey))
            {
                throw new ArgumentNullException(nameof(strKey));
            }

            object cached;
            return await Task.Run(() => cache.TryGetValue(strKey, out cached));
        }

        #endregion

        #region 添加缓存

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string strKey, object value)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            cache.Set(strKey, value);
            return Exists(strKey);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="strKey">缓存key</param>
        /// <param name="value">缓存value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiresAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string strKey, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            cache.Set(strKey, value,
                new MemoryCacheEntryOptions()
                .SetSlidingExpiration(expiresSliding)
                .SetAbsoluteExpiration(expiresAbsoulte));
            return Exists(strKey);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="strKey">缓存key</param>
        /// <param name="value">缓存value</param>
        /// <param name="expiresIn">过期时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Add(string strKey, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isSliding)
            {
                cache.Set(strKey, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresIn));
            }
            else
            {
                cache.Set(strKey, value,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expiresIn));
            }
            return Exists(strKey);
        }

        /// <summary>
        /// 添加缓存（异步）
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string strKey, object value)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            await Task.Run(()=>cache.Set(strKey, value));
            return await ExistsAsync(strKey);
        }

        /// <summary>
        /// 添加缓存（异步）
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulte"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string strKey, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            await Task.Run(() =>
                cache.Set(strKey, value,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(expiresSliding)
                        .SetAbsoluteExpiration(expiresAbsoulte)));
            return await ExistsAsync(strKey);
        }

        /// <summary>
        /// 添加缓存（异步）
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <param name="isSliding"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string strKey, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (isSliding)
            {
                await Task.Run(() =>
                    cache.Set(strKey, value,
                        new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(expiresIn)));
            }
            else
            {
                await Task.Run(() =>
                    cache.Set(strKey, value,
                        new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(expiresIn)));
            }
            return await ExistsAsync(strKey);
        }

        #endregion

        #region 获取缓存

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public T Get<T>(string strKey) where T : class
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            return cache.Get(strKey) as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public object Get(string strKey)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            return cache.Get(strKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            IDictionary<string, object> dic = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dic.Add(item, cache.Get(item)));
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            IDictionary<string, object> dic = new Dictionary<string, object>();
            await Task.Run(() => keys.ToList().ForEach(item => dic.Add(item, cache.Get(item))));
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string strKey) where T : class
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            return await Task.Run(() => cache.Get(strKey) as T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(string strKey)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            return await Task.Run(() => cache.Get(strKey));
        }

        #endregion

        #region 删除缓存

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public bool Remove(string strKey)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            cache.Remove(strKey);
            return !Exists(strKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            keys.ToList().ForEach(item => cache.Remove(item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            await Task.Run(() => keys.ToList().ForEach(item => cache.Remove(item)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string strKey)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }

            await Task.Run(() => cache.Remove(strKey));

            return await ExistsAsync(strKey) ? false : true;
        }

        #endregion

        #region 修改缓存

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulte"></param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return Add(key, value, expiresSliding, expiresAbsoulte);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <param name="isSliding"></param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return Add(key, value, expiresIn, isSliding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return await AddAsync(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulte"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return await AddAsync(key, value, expiresSliding, expiresAbsoulte);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <param name="isSliding"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return await AddAsync(key, value, expiresIn, isSliding);
        }

        #endregion

        #region 释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (cache != null)
            {
                cache.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
