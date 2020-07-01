using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GYLib.Base.Utils;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;



namespace HZY.Toolkit.ToolKits
{
    public class LKRedisHelper
    {



        public static IConfiguration configuration { get; set; }
        private readonly static string CONNECTION_REDISVALIDTIME = "RedisInvalidTime";
        private readonly static string CONNECTION_REDIS = "RedisConnection";

        private static string _conn = "";
        //ConvertUtils.ConvertToString(ConfigurationHelper.GetAppSettings<RedisConnectionStrings>("RedisConnectionStrings").RedisConnection);


        // "192.168.173.7";

        public static int _RedisInvalidTime = 0;


        public static void LoadConnection()
        {
            if (string.IsNullOrEmpty(_conn) || _RedisInvalidTime <= 0)
            {
                //todo   
                _conn = "192.168.173.7";//ConvertUtils.ConvertToString(ConfigurationHelper.GetAppSettings<RedisConnectionStrings>("RedisConnectionStrings").LKRedisConnection);
                _RedisInvalidTime = 30; //ConvertUtils.ConvertToInt(ConfigurationHelper.GetAppSettings<RedisConnectionStrings>("RedisConnectionStrings").LKRedisInvalidTime);

            }


        }





        /// <summary>
        /// 单条值存入redis
        /// </summary>
        /// <param name="rediskey"></param>
        /// <param name="redisvalue"></param>
        /// <param name="timespanminutes">默认5分钟过期</param>
        /// <param name="IsExpire">如果IsExpire=false 表示不过期</param>
        /// <returns></returns>
        public static bool RedisSetData(string rediskey, string redisvalue, bool IsExpire = true, int timespanminutes = 5)
        {
            try
            {
                LoadConnection();
                using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_conn))
                {
                    IDatabase db = conn.GetDatabase(1, null);
                    //如果IsExpire=false 表示不过期
                    if (!IsExpire)
                    {
                        return db.StringSet(rediskey, redisvalue, TimeSpan.MaxValue, When.Always, CommandFlags.None);
                    }
                    else
                    {
                        return db.StringSet(rediskey, redisvalue, TimeSpan.FromMinutes(timespanminutes), When.Always, CommandFlags.None);
                    }

                }
            }
            catch (Exception ex)
            {

                //LogHelp.Error(ex.Message);
                return false;
            }


        }

        /// <summary>
        /// 多条值存入redis
        /// </summary>
        /// <param name="rediskey"></param>
        /// <param name="redisvalue"></param>
        /// <param name="timespanminutes"></param>
        public static bool RedisSetManyData(string[] rediskey, string[] redisvalue, bool IsExpire = true, int timespanminutes = 5)
        {
            try
            {
                LoadConnection();
                timespanminutes = (timespanminutes > _RedisInvalidTime ? _RedisInvalidTime : timespanminutes);
                var count = rediskey.Length;
                if (count > 0)
                {
                    var keyvaluepair = new KeyValuePair<RedisKey, RedisValue>[count];
                    for (int i = 0; i < count; i++)
                    {
                        keyvaluepair[i] = new KeyValuePair<RedisKey, RedisValue>(rediskey[i], redisvalue[i]);
                    }

                    using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_conn))
                    {
                        IDatabase db = conn.GetDatabase(1, null);
                        return db.StringSet(keyvaluepair);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                //LogHelp.Error(ex.Message);
                return false;
            }


        }




        /// <summary>
        /// redis查询单条值
        /// </summary>
        /// <param name="rediskey"></param>
        /// <returns></returns>
        public static string RedisStringGet(string rediskey)
        {
            try
            {

                LoadConnection();
                using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_conn))
                {
                    IDatabase db = conn.GetDatabase(1, null);
                    if (db.KeyExists(rediskey, CommandFlags.None))
                    {
                        return db.LockQuery(rediskey);
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelp.Error(ex.Message);

            }
            return null;
        }




        /// <summary>
        /// redis查询单条值
        /// </summary>
        /// <param name="rediskey"></param>
        /// <returns></returns>
        public static string[] RedisStringGetMany(string[] rediskey)
        {
            try
            {
                LoadConnection();
                var count = rediskey.Length;
                var keys = new RedisKey[count];
                var addrs = new string[count];

                for (var i = 0; i < count; i++)
                {
                    keys[i] = rediskey[i];
                }
                using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_conn))
                {
                    IDatabase db = conn.GetDatabase(1, null);
                    var values = db.StringGet(keys);
                    for (var i = 0; i < values.Length; i++)
                    {
                        if (db.KeyExists(keys[i], CommandFlags.None))
                        {
                            addrs[i] = values[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelp.Error(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 根据key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteRedisByKey(string key)
        {
            try
            {
                using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_conn))
                {
                    IDatabase db = conn.GetDatabase(1, null);
                    return db.KeyDelete(key);
                }
            }
            catch (Exception ex)
            {
                //LogHelp.Error(ex.Message);
                return false;
            }

        }




        #region 泛型
        /// <summary>
        /// 存值并设置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        /// <param name="IsExpire">如果IsExpire=false 表示不过期</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T t, bool IsExpire = true, int timespanminutes = 5)
        {
            try
            {

                LoadConnection();
                //timespanminutes = (timespanminutes > _RedisInvalidTime ? _RedisInvalidTime : timespanminutes);
                var str = JsonConvert.SerializeObject(t);
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    
                    IDatabase db = client.GetDatabase(1, null);
                   
                    TimeSpan span = new TimeSpan(0, 0, timespanminutes, 0);
                    return db.StringSet(key, str, (IsExpire == false ? TimeSpan.MaxValue : span));
                }
            }
            catch (Exception ex)
            {
                //LogHelp.Error(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// 根据Key获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                LoadConnection();
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    IDatabase db = client.GetDatabase(1, null);
                    var strValue = db.StringGet(key);
                    return string.IsNullOrEmpty(strValue) ? null : JsonConvert.DeserializeObject<T>(strValue);
                }
            }
            catch (Exception ex)
            {

                //LogHelp.Error(ex.Message);
                return null;
            }

        }
        #endregion





    }

}
