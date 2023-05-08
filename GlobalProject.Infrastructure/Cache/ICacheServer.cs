using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure
{
  /// <summary>
  /// 功能描述    ：ICacheServer  
  /// 创 建 者    ：weitao du
  /// 创建日期    ：2022/9/8 9:40:14 
  /// 最后修改者  ：admin
  /// 最后修改日期：2022/9/8 9:40:14 
  /// </summary>
  public interface ICacheServer
  {
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public bool Set(string key, string value);
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="cacheDurationInSeconds"></param>
    public bool Set(string key, string value, int cacheDurationInSeconds);
    /// <summary>
    /// 判断是否包含缓存key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(string key);
    /// <summary>
    /// 获取缓存key
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public V Get<V>(string key) where V : class;
    /// <summary>
    /// 获取缓存key
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string key);
    /// <summary>
    /// 获取所有的缓存key
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    public IEnumerable<string> GetAllKey<V>();
    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    public bool Del(string key);
    /// <summary>
    /// 根据parrent清除缓存
    /// </summary>
    /// <param name="parrent"></param>
    /// <returns></returns>
    public bool RemoveCacheRegex(string parrent);
  }
}
