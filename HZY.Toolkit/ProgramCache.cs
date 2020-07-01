using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit
{
    using System.Collections.Concurrent;

    /// <summary>
    /// 内存缓存 游戏 数据 缓存 操作类
    /// </summary>
    public class ProgramCache
    {
        /// <summary>
        /// 容器锁
        /// </summary>
        //private static object _objectlock = new object();
        /// <summary>
        /// 数据容器
        /// </summary>
        private readonly static ConcurrentDictionary<string, object> _ConcurrentDictionary = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 根据Key 获取 游戏树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static T Get<T>(string Key)
        {
            lock (_ConcurrentDictionary)
            {
                _ConcurrentDictionary.TryGetValue(Key, out object _Value);
                if (_Value == null) _Value = default(T);
                return (T)_Value;
            }
        }

        /// <summary>
        /// 获取 所有的 游戏树
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, object> GetAll() => _ConcurrentDictionary;

        /// <summary>
        /// 根据Key 移除 游戏树
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool Remove(string Key)
        {
            lock (_ConcurrentDictionary) return _ConcurrentDictionary.TryRemove(Key, out object _Value);
        }

        /// <summary>
        /// 清除所有 游戏树
        /// </summary>
        public static void Clear()
        {
            lock (_ConcurrentDictionary) _ConcurrentDictionary.Clear();
        }

        /// <summary>
        /// 添加游戏树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Data"></param>
        public static bool Add<T>(string Key, T Data)
        {
            lock (_ConcurrentDictionary) return _ConcurrentDictionary.TryAdd(Key, Data);
        }

        /// <summary>
        /// 更新游戏树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static bool Update<T>(string Key, T Data)
        {
            lock (_ConcurrentDictionary) return _ConcurrentDictionary.TryUpdate(Key, Data, Get<T>(Key));
        }

        /// <summary>
        /// 添加或者更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static bool AddOrUpdate<T>(string Key, T Data)
        {
            if (ContainsKey(Key))
                return Update<T>(Key, Data);
            return Add<T>(Key, Data);
        }

        /// <summary>
        /// 键名是否存在
        /// </summary>
        /// <param name="Key"></param>
        public static bool ContainsKey(string Key)
        {
            lock (_ConcurrentDictionary) return _ConcurrentDictionary.ContainsKey(Key);
        }


    }
}
