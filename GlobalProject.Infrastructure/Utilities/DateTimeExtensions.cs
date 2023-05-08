using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public static class DateTimeExtensions
  {
    /// <summary>
    /// 日期字符串精确到天,yyyy.MM.dd
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToAccuratetDayString(this DateTime date)
    {
      return date.ToString("yyyy.MM.dd");
    }
    /// <summary>
    /// 日期字符串精确到时,yyyy.MM.dd HH
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToAccurateHourString(this DateTime date)
    {
      return date.ToString("yyyy.MM.dd HH");
    }
    /// <summary>
    /// 日期字符串精确到分,yyyy.MM.dd HH:mm
    /// </summary>
    /// <param name="date"></param>
    /// <returns>yyyy.MM.dd HH:mm</returns>
    public static string ToAccurateMinuteString(this DateTime date)
    {
      return date.ToString("yyyy.MM.dd HH:mm");
    }
    /// <summary>
    /// 自定义格式
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToCustomString(this DateTime date, string format)
    {
      return date.ToString($"{format}");
    }
  }    
}
