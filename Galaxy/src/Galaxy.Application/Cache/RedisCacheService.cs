using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace Galaxy.Cache
{
    /// <summary>
    /// 缓存操作实现类RedisCacheService
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase cache;
        private readonly ConnectionMultiplexer connection;
        private readonly string instance;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="options"></param>
        /// <param name="database"></param>
        public RedisCacheService(RedisCacheOptions options, int database = 0)
        {
            connection = ConnectionMultiplexer.Connect(options.Configuration);
            cache = connection.GetDatabase(database);
            instance = options.InstanceName;
        }

        #region 组合Key值和实例名

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKeyForRedis(string key)
        {
            return instance + key;
        }

        #endregion

        #region 验证缓存是否存在

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public bool Exists(string strKey)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            return cache.KeyExists(GetKeyForRedis(strKey));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string strKey)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            return await cache.KeyExistsAsync(GetKeyForRedis(strKey));
        }

        #endregion

        #region 添加缓存

        /// <summary>
        /// 
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

            return cache.StringSet(GetKeyForRedis(strKey), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulte"></param>
        /// <returns></returns>
        public bool Add(string strKey, object value, TimeSpan expiresSliding, TimeSpan expiresAbsoulte)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }
            
            return cache.StringSet(GetKeyForRedis(strKey), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresAbsoulte);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <param name="isSliding"></param>
        /// <returns></returns>
        public bool Add(string strKey, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (strKey == null)
            {
                throw new ArgumentNullException(nameof(strKey));
            }

            return cache.StringSet(GetKeyForRedis(strKey), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
        }

        /// <summary>
        /// 
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

            return await cache.StringSetAsync(GetKeyForRedis(strKey), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 
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

            return await cache.StringSetAsync(GetKeyForRedis(strKey), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresAbsoulte);
        }

        /// <summary>
        /// 
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

            return await cache.StringSetAsync(GetKeyForRedis(strKey), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
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
            var value = cache.StringGet(GetKeyForRedis(strKey));
            if (!value.HasValue)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
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
            var value = cache.StringGet(GetKeyForRedis(strKey));
            if (!value.HasValue)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value);
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
            var dic = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dic.Add(item, Get(GetKeyForRedis(item))));

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
            var dic = new Dictionary<string, object>();
            await Task.Run(() => keys.ToList().ForEach(item => dic.Add(item, Get(GetKeyForRedis(item)))));

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
            var value = await cache.StringGetAsync(GetKeyForRedis(strKey));
            if (!value.HasValue)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
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
            var value = await cache.StringGetAsync(GetKeyForRedis(strKey));
            if (!value.HasValue)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value);
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
            return cache.KeyDelete(GetKeyForRedis(strKey));
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

            keys.ToList().ForEach(item => Remove(item));
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

            await Task.Run(() => keys.ToList().ForEach(item => Remove(item)));
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
            return await cache.KeyDeleteAsync(GetKeyForRedis(strKey));
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
            if (Exists(key))
            {
                if (!Remove(key))
                {
                    return false;
                }
            }
            return await AddAsync(key, value, expiresSliding, expiresAbsoulte);
        }

        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
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
            if (connection != null)
            {
                connection.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
