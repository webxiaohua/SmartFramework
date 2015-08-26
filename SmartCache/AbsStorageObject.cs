using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.CacheFunction
{
    /// <summary>
    /// @Author:Robin
    /// @Date:2015-08-27
    /// @Desc:存储对象接口
    /// </summary>
    public abstract class AbsStorageObject<T>
    {
        public int Minutes = 60;
        public int Hour = 60 * 60;
        public int Day = 60 * 60 * 24;
        public System.Web.HttpContext context = System.Web.HttpContext.Current;
        public abstract void Add(string key, T value);
        public abstract bool ContainsKey(string key);
        public abstract T Get(string key);
        public abstract IEnumerable<string> GetAllKey();
        public abstract void Remove(string key);
        public abstract void RemoveAll();
        public abstract void RemoveAll(Func<string, bool> removeExpression);
        public abstract T this[string key] { get; }
    }
}
