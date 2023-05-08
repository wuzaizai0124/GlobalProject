
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure
{
  public static class SmsServiceExtension
  {
    public static IServiceCollection AddSmsService(this IServiceCollection services, IConfiguration configuration)
    {
      var config = configuration.GetSection("SmsConfig");
      services.Configure<SmsConfig>(options =>
      {
        options.Sn = config["Sn"];
        options.Pwd = config["Pwd"];
        options.Signature = config["Signature"];
        options.Url = config["Url"];
      });
      services.AddSingleton<ISmsService, SmsService>();
      return services;
    }
  }


  public class SmsService : ISmsService
  {
    private SmsConfig _config;
    public SmsService(IOptions<SmsConfig> options)
    {
      _config = options.Value;
    }
    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    public int GetCode()
    {
      Random random = new Random();
      var code = random.Next(100000, 999999);
      return code;
    }
    /// <summary>
    /// 发送短信
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public bool SendCode(string content, string phone)
    {
      string password = MD5Helper.GetMD5($"{_config.Sn}{_config.Pwd}");
      string uri = string.Format("{0}/mdsmssend.ashx?sn={1}&pwd={2}&mobile={3}&content={4}{5}&stime=&rrid=&msgfmt=",
               _config.Url, _config.Sn, password, phone, _config.Signature, content);
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
      string result = "";
      try
      {
        using (WebResponse wr = request.GetResponse())
        {
          StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.UTF8);
          result = sr.ReadToEnd().Trim();
        }
        long code = long.Parse(result);
        return code > 0;
      }
      catch (Exception e)
      {
        //e 记录异常日志使用
        return false;
      }
    }

    public bool SendMessage(string content, string phone)
    {
      string password = MD5Helper.GetMD5($"{_config.Sn}{_config.Pwd}");
      string uri = string.Format("{0}/mdsmssend.ashx?sn={1}&pwd={2}&mobile={3}&content={4}{5}&stime=&rrid=&msgfmt=",
               _config.Url, _config.Sn, password, phone, _config.Signature, content);
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
      string result = "";
      try
      {
        using (WebResponse wr = request.GetResponse())
        {
          StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.UTF8);
          result = sr.ReadToEnd().Trim();
        }
        long code = long.Parse(result);
        return code > 0;
      }
      catch (Exception e)
      {
        //e 记录异常日志使用
        return false;
      }
    }
  }

  public interface ISmsService
  {
    bool SendMessage(string content, string phone);
    bool SendCode(string content, string phone);
    int GetCode();
  }
  public class SmsConfig
  {
    public string Sn { get; set; }
    public string Pwd { get; set; }
    public string Signature { get; set; }
    public string Url { get; set; }
  }

}
