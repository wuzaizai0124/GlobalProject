using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public static class MD5Helper
  {
    /// <summary>
    /// 16位MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Encrypt16(string str)
    {
      var md5 = new MD5CryptoServiceProvider();
      string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)), 4, 8);
      t2 = t2.Replace("-", "");
      return t2;
    }

    /// <summary>
    /// 32位MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Encrypt32(string str)
    {
      string cl = str;
      string pwd = "";
      MD5 md5 = MD5.Create(); //实例化一个md5对像
                              // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
      byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
      // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
      for (int i = 0; i < s.Length; i++)
      {
        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
        pwd = pwd + s[i].ToString("X");
      }
      return pwd;
    }

    /// <summary>
    /// 64位MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Encrypt64(string str)
    {
      string cl = str;
      MD5 md5 = MD5.Create(); //实例化一个md5对像
                              // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
      byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
      return Convert.ToBase64String(s);
    }

    //md5散列加密
    public static string GetMD5(string str)
    {
      if (String.IsNullOrEmpty(str))
        return str;
      try
      {
        var sb = new StringBuilder(32);
        var md5 = MD5.Create();
        var output = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
        for (int i = 0; i < output.Length; i++)
          sb.Append(output[i].ToString("X").PadLeft(2, '0'));
        return sb.ToString();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
