using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Web.Caching;

namespace Smart.CacheFunction
{
    /// <summary>
    /// @Author:Robin
    /// @Date:2015-08-27
    /// @Desc:缓存操作类
    /// </summary>
    public class CacheHelper<T> : AbsStorageObject<T>
    {
        #region 全局变量
        private static CacheHelper<T> _instance = null;
        private static readonly object _instanceLock = new object();
        #endregion

        #region 构造函数
        private CacheHelper() { }
        #endregion

        #region  属性
        /// <summary>         
        ///根据key获取value     
        /// </summary>         
        /// <value></value>      
        public override T this[string key]
        {
            get { return (T)HttpRuntime.Cache[CreateKey(key)]; }
        }
        #endregion


        #region 公共函数
        /// <summary>         
        /// 验证key是否存在       
        /// </summary>         
        /// <param name="key">key</param>         
        /// <returns> /// 	存在<c>true</c> 不存在<c>false</c>.        /// /// </returns>         
        public override bool ContainsKey(string key)
        {
            return HttpRuntime.Cache[CreateKey(key)] != null;
        }
        /// <summary>         
        /// 根据key获取value  
        /// </summary>         
        /// <param name="key">key</param>         
        /// <returns></returns>         
        public override T Get(string key)
        {
            return (T)HttpRuntime.Cache.Get(CreateKey(key));
        }
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>         
        public static CacheHelper<T> GetInstance()
        {
            if (_instance == null)
                lock (_instanceLock)
                    if (_instance == null)
                        _instance = new CacheHelper<T>();
            return _instance;
        }
        /// <summary>         
        /// 插入缓存(默认20分钟)        
        /// </summary>         
        /// <param name="key"> key</param>         
        /// <param name="value">value</param>          
        public override void Add(string key, T value)
        {
            Add(key, value, Minutes * 20);
        }
        /// <summary>         
        /// 插入缓存        
        /// </summary>         
        /// <param name="key"> key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        public void Add(string key, T value, int cacheDurationInSeconds)
        {
            Add(key, value, cacheDurationInSeconds, CacheItemPriority.Default);
        }
        /// <summary>         
        /// 插入缓存.         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        /// <param name="priority">缓存项属性</param>         
        public void Add(string key, T value, int cacheDurationInSeconds, CacheItemPriority priority)
        {
            string keyString = CreateKey(key);
            HttpRuntime.Cache.Insert(keyString, value, null, DateTime.Now.AddSeconds(cacheDurationInSeconds), Cache.NoSlidingExpiration, priority, null);
        }
        /// <summary>         
        /// 插入缓存.         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        /// <param name="priority">缓存项属性</param>         
        public void Add(string key, T value, int cacheDurationInSeconds, CacheDependency dependency, CacheItemPriority priority)
        {
            string keyString = CreateKey(key);
            HttpRuntime.Cache.Insert(keyString, value, dependency, DateTime.Now.AddSeconds(cacheDurationInSeconds), Cache.NoSlidingExpiration, priority, null);
        }
        /// <summary>         
        /// 删除缓存         
        /// </summary>         
        /// <param name="key">key</param>         
        public override void Remove(string key)
        {
            HttpRuntime.Cache.Remove(CreateKey(key));
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public override void RemoveAll()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }
            foreach (string key in al)
            {
                cache.Remove(key);
            }
        }
        /// <summary>
        /// 清除所有包含关键字的缓存
        /// </summary>
        /// <param name="removeKey">关键字</param>
        public override void RemoveAll(Func<string, bool> removeExpression)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            var allKeyList = GetAllKey();
            var delKeyList = allKeyList.Where(removeExpression).ToList();
            foreach (var key in delKeyList)
            {
                Remove(key);
            }
        }
        /// <summary>
        /// 获取所有缓存key
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetAllKey()
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                yield return CacheEnum.Key.ToString();
            }
        }
        #endregion

        #region 私有函数
        /// <summary>         
        ///创建KEY   
        /// </summary>         
        /// <param name="key">Key</param>         
        /// <returns></returns>         
        private string CreateKey(string key)
        {
            return "Smart." + key.ToString();
        }
        #endregion
    }
}
