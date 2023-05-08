using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace GlobalProject.Infrastructure
{
  public static class StringExitension
  {
    /// <summary>
    /// 判断是否是手机号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsPhone(this string str)
    {
      if (string.IsNullOrWhiteSpace(str))
        return false;
      var regxStr = @"/^1[3-9]\d{9}$/";
      var regex = new Regex(regxStr);
      return regex.IsMatch(str);
    }
    /// <summary>
    /// 判断是否为空或者空字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(string str)
    {
      return string.IsNullOrWhiteSpace(str);
    }
  }
}
