using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace GlobalProject.Infrastructure.Utilities
{
    public class DeleteData
  {
    public int ResumeId { get; set; }
  }
  public static class ElasticsearchHelper
  {

 
    /// <summary>
    /// 删除简历
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="staffId"></param>
    /// <returns></returns>
    public static string Delete(int resumeId, int staffId, IConfiguration configuration,string token)
    {
      var es_url = configuration.GetSection("es_url").Value;
      //临时注释  ES恢复后  解注释 20211104
      //return "";
      try
      {
        DeleteData data = new DeleteData();
        data.ResumeId = resumeId;
        var result = Post(es_url + "/Resume/delete", JsonConvert.SerializeObject(data), token);
        return result;
      }
      catch (Exception)
      {
        return "请求失败";
      }
      //return new BaseResponse { Message = qp };
    }
    ///// <summary>
    ///// 获取token
    ///// </summary>
    ///// <param name="staffid"></param>
    ///// <returns></returns>
    //public static string GetResumeToken(int staffid)
    //{
    //  using (var ctx = new HeadHunterEntities())
    //  {
    //    var token = ctx.ESTokens.Where(s => s.StaffId == staffid).FirstOrDefault();
    //    if (token == null)
    //    {
    //      ESToken rt = new ESToken();
    //      rt.StaffId = staffid;
    //      rt.Token = GetRandomString(30, true, true, true, "risfond");
    //      rt.CreateTime = DateTime.Now;
    //      ctx.ESTokens.Add(rt);
    //      ctx.SaveChanges();
    //      return rt.Token;
    //    }
    //    DateTime dt = DateTime.Now;
    //    if (token.CreateTime.Date < dt.Date)
    //    {
    //      token.Token = GetRandomString(30, true, true, true, "risfond");
    //      token.CreateTime = dt;
    //      ctx.SaveChanges();
    //    }
    //    return token.Token;
    //  }
    //}
    ///<summary>
    ///生成随机字符串 
    ///</summary>
    ///<param name="length">目标字符串的长度</param>
    ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
    ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
    ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
    ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
    ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
    ///<returns>指定长度的随机字符串</returns>
    public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, string custom)
    {
      byte[] b = new byte[4];
      new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
      Random r = new Random(BitConverter.ToInt32(b, 0));
      string s = null, str = custom;
      if (useNum == true) { str += "0123456789"; }
      if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
      if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
      for (int i = 0; i < length; i++)
      {
        s += str.Substring(r.Next(0, str.Length - 1), 1);
      }
      return s;
    }
    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url">请求接口地址</param>
    /// <param name="dataSource">请求参数（json）</param>
    /// <param name="token">Resumetoken</param>
    /// <returns></returns>
    public static string Post(string url, string dataSource, string token)
    {
      try
      {
        //Http协议内容 使用ASCII码字符
        byte[] data = Encoding.UTF8.GetBytes(dataSource);
        //发送信息
        HttpWebRequest req = WebRequest.CreateHttp(url);
        req.Method = "POST";
        req.ContentType = "application/json; charset=utf-8";
        req.Headers.Add("token", token);
        //添加Post参数
        //req.ContentLength = dataSource.Length;
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(data, 0, data.Length);
        reqStream.Close();
        //接收相应
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream respStream = resp.GetResponseStream();
        StreamReader reader = new StreamReader(respStream, Encoding.GetEncoding("utf-8"));
        string retString = reader.ReadToEnd();
        reader.Close();
        return retString;
      }
      catch (Exception ex)
      {
        return "{ret:'0',msg:" + ex.Message + "}";
      }
    }

  }
}
