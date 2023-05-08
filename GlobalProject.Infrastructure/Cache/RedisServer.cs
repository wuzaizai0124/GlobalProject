using CSRedis;
using GlobalProject.Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace GlobalProject.Infrastructure
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public class RedisServer : ICacheServer
    {
        private static CSRedisClient client;
        //锁
        private static object _redisLock = new object();
        public RedisServer()
        {
            //初始化数据
            Initalize();
        }
        /// <summary>
        /// 初始化redis实例
        /// </summary>
        private static void Initalize()
        {
            if (client == null)
            {
                lock (_redisLock)
                {
                    if (client == null)
                    {
                        var configuration = EngineContext.Current.Resolve<IConfiguration>();
                        client = new CSRedisClient(configuration.GetSection("RedisServer")["Client"]);
                    }
                }
            }

        }
        public static CSRedisClient Instanse()
        {
            return client;
        }
        public bool Set(string key, string value)
        {
            return Instanse().Set(key, value);
        }

        public bool Set(string key, string value, int cacheDurationInSeconds)
        {
            return Instanse().Set(key, value, cacheDurationInSeconds);

        }

        public bool ContainsKey(string key)
        {
            return Instanse().Exists(key);
        }

        public V Get<V>(string key) where V : class
        {
            return Instanse().Get<V>(key);
        }
        public string Get(string key)
        {
            return Instanse().Get(key);
        }
        public IEnumerable<string> GetAllKey<V>()
        {
            return Instanse().Keys("");
        }
        public bool Del(string key)
        {
            return Instanse().Del(key) > 0;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="parrent"></param>
        /// <returns></returns>
        public bool RemoveCacheRegex(string parrent)
        {
            //
            string[] keys = Instanse().Keys(parrent + "*");
            //
            if (keys.Count() > 0)
            {
                Instanse().Del(keys);
            }
            return true;
        }
    }
}
