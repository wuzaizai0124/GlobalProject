using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public class HttpHelper
  {
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
    /// <summary>  
    /// 创建GET方式的HTTP请求  
    /// </summary>  
    /// <param name="url">请求的URL</param>  
    /// <param name="timeout">请求的超时时间</param>  
    /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
    /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
    /// <returns></returns>  
    public static string CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
    {
      if (string.IsNullOrEmpty(url))
      {
        throw new ArgumentNullException("url");
      }
      HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
      request.Method = "GET";
      request.UserAgent = DefaultUserAgent;
      if (!string.IsNullOrEmpty(userAgent))
      {
        request.UserAgent = userAgent;
      }
      if (timeout.HasValue)
      {
        request.Timeout = timeout.Value;
      }
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      var response = request.GetResponse() as HttpWebResponse;
      StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      string responseContent = streamReader.ReadToEnd();
      streamReader.Close();
      response.Close();
      return responseContent;
    }
    /// <summary>  
    /// 创建POST方式的HTTP请求  
    /// </summary>  
    /// <param name="url">请求的URL</param>  
    /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
    /// <param name="timeout">请求的超时时间</param>  
    /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
    /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
    /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
    /// <returns></returns>  
    public static string CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies, Dictionary<string, object> heades)
    {
      if (string.IsNullOrEmpty(url))
      {
        throw new ArgumentNullException("url");
      }
      if (requestEncoding == null)
      {
        throw new ArgumentNullException("requestEncoding");
      }
      HttpWebRequest request = null;
      //如果是发送HTTPS请求  
      if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
      {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        request = WebRequest.Create(url) as HttpWebRequest;
        request.ProtocolVersion = HttpVersion.Version10;
      }
      else
      {
        request = WebRequest.Create(url) as HttpWebRequest;
      }
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      if (heades?.Count > 0)
      {
        foreach (var item in heades)
        {
          request.Headers.Add(item.Key, item.Value?.ToString());
        }
      }
      if (!string.IsNullOrEmpty(userAgent))
      {
        request.UserAgent = userAgent;
      }
      else
      {
        request.UserAgent = DefaultUserAgent;
      }

      if (timeout.HasValue)
      {
        request.Timeout = timeout.Value;
      }
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      //如果需要POST数据  
      if (!(parameters == null || parameters.Count == 0))
      {
        StringBuilder buffer = new StringBuilder();
        int i = 0;
        foreach (string key in parameters.Keys)
        {
          if (i > 0)
          {
            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
          }
          else
          {
            buffer.AppendFormat("{0}={1}", key, parameters[key]);
          }
          i++;
        }
        byte[] data = requestEncoding.GetBytes(buffer.ToString());
        using (Stream stream = request.GetRequestStream())
        {
          stream.Write(data, 0, data.Length);
        }
      }
      var response = request.GetResponse() as HttpWebResponse;
      StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      string responseContent = streamReader.ReadToEnd();
      streamReader.Close();
      response.Close();
      return responseContent;
    }
    /// <summary>  
    /// 创建POST方式的HTTP请求  
    /// </summary>  
    /// <param name="url">请求的URL</param>  
    /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
    /// <param name="timeout">请求的超时时间</param>  
    /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
    /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
    /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
    /// <returns></returns>  
    public static string CreatePostHttpResponse(string url, string postData, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies, Dictionary<string, object> heades, string ContentType = "application/x-www-form-urlencoded")
    {
      if (string.IsNullOrEmpty(url))
      {
        throw new ArgumentNullException("url");
      }
      if (requestEncoding == null)
      {
        throw new ArgumentNullException("requestEncoding");
      }
      HttpWebRequest request = null;
      //如果是发送HTTPS请求  
      if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
      {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        request = WebRequest.Create(url) as HttpWebRequest;
        request.ProtocolVersion = HttpVersion.Version10;
      }
      else
      {
        request = WebRequest.Create(url) as HttpWebRequest;
      }
      request.Method = "POST";
      request.ContentType = ContentType;
      if (heades?.Count > 0)
      {
        foreach (var item in heades)
        {
          request.Headers.Add(item.Key, item.Value?.ToString());
        }
      }
      if (!string.IsNullOrEmpty(userAgent))
      {
        request.UserAgent = userAgent;
      }
      else
      {
        request.UserAgent = DefaultUserAgent;
      }

      if (timeout.HasValue)
      {
        request.Timeout = timeout.Value;
      }
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      //如果需要POST数据  
      if (string.IsNullOrEmpty(postData) == false)
      {
        byte[] data = requestEncoding.GetBytes(postData);
        using (Stream stream = request.GetRequestStream())
        {
          stream.Write(data, 0, data.Length);
        }
      }
      var response = request.GetResponse() as HttpWebResponse;
      StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      string responseContent = streamReader.ReadToEnd();
      streamReader.Close();
      response.Close();
      return responseContent;
    }

    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
      return true; //总是接受  
    }
    public static string PostMoths(string url, string param)
    {
      string strURL = url;
      System.Net.HttpWebRequest request;
      request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
      request.Method = "POST";
      request.ContentType = "application/json;charset=UTF-8";
      string paraUrlCoded = param;
      byte[] payload;
      payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
      request.ContentLength = payload.Length;
      Stream writer = request.GetRequestStream();
      writer.Write(payload, 0, payload.Length);
      writer.Close();
      System.Net.HttpWebResponse response;
      response = (System.Net.HttpWebResponse)request.GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      string responseContent = streamReader.ReadToEnd();
      streamReader.Close();
      response.Close();
      return responseContent;
    }
  }
}
