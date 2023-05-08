using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using GlobalProject.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject
{
  /// <summary>
  /// 功能描述    ：HttpContextCore  
  /// 创 建 者    ：admin
  /// 创建日期    ：2022/7/21 22:26:44 
  /// 最后修改者  ：admin
  /// 最后修改日期：2022/7/21 22:26:44 
  /// </summary>
  public static class HttpContextCore
  {
    public static IServiceProvider ServiceProvider { get; set; }
    public static IServiceCollection Service { get; set; }

    /// <summary>
    /// 获取请求内容
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<string> GetRequest(HttpRequest request, string parameter)
    {
      if (parameter.IndexOf("|") > -1)
        parameter = parameter.Split('|')[0];
      // 启用倒带功能，就可以让 Request.Body 可以再次读取
      string resultStr = string.Empty;
      if (request.Method.ToUpper() == "POST")
      {
        Stream stream = request.Body;
        byte[] buffer = new byte[request.ContentLength.Value];
        await stream.ReadAsync(buffer, 0, buffer.Length);
        request.Body.Position = 0;
        string body = Encoding.UTF8.GetString(buffer).ToLower();
        if (string.IsNullOrEmpty(body) == false)
        {
          if (body.IndexOf(parameter) > -1)
          {
            resultStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(body)[parameter]?.ToString();
          }
        }
      }
      else if (request.Method.ToUpper() == "GET")
      {
        resultStr = request.Query[parameter].ToString();
      }
      return resultStr;
    }
    /// <summary>
    /// 获取请求参数
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static string GetParams(string parameter, string query)
    {
      parameter = parameter.ToLower();
      query = query.ToLower();
      if (query.IndexOf(parameter) > -1)
      {
        int nextParamsIndex = query.IndexOf("&", query.IndexOf(parameter));
        int nextParamsEquasIndex = query.IndexOf("=", query.IndexOf(parameter));
        if (nextParamsIndex == -1)
        {
          return query.Substring(query.IndexOf(parameter) + parameter.Length + 1);
        }
        else
        {
          return query.Substring(query.IndexOf(parameter) + parameter.Length + 1, nextParamsIndex - nextParamsEquasIndex - 1);
        }
      }
      return string.Empty;
    }
    /// <summary>
    /// 获取referer参数
    /// </summary>
    /// <param name="context"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string UrlParams(this HttpContext context, string param)
    {
      if (!string.IsNullOrEmpty(context.Request.Headers["referer"]))
      {
        Uri refererUrl = new Uri(context.Request.Headers["referer"]);
        return GetParams(param, refererUrl.Query);
      }
      return string.Empty;
    }
    /// <summary>
    /// 获取当前请求的域名
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string GetAbsoluteUri(this HttpRequest request,string path="")
    {
      return new StringBuilder()
          .Append(request.Scheme)
          .Append("://")
          .Append(request.Host)
          .Append(request.PathBase)
          .Append(path)
          .ToString();
    }
  }
}
