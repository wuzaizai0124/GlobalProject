using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure.Utilities
{
  public class StaffHelper
  {
    public static string DateDiff(DateTime start, DateTime end)
    {
      TimeSpan ts = end - start;
      int year = 0, month = 0, day = ts.Days;
      int temp = 0;
      //四年一周期
      temp = day / (365 * 3 + 366);
      year = temp * 4;
      day = day % (365 * 3 + 366);
      //一年一周期
      temp = day / 365;
      year += temp;
      day = day % 365;
      //一月一周期，由于每月天数不一样，这个只能逐月推算了
      DateTime mi = start.AddYears(year);
      while ((mi = mi.AddMonths(1)) <= end)
      {
        month++;
      }
      //逐天推算，直接相减
      day = (end - mi.AddMonths(-1)).Days;
      StringBuilder sb = new StringBuilder();
      if (year > 0)
        sb.Append(string.Format("{0}年", year));
      if (month > 0)
        sb.Append(string.Format("{0}个月", month));
      if (day > 0)
        sb.Append(string.Format("{0}天", day));
      var result = sb.ToString();
      if (string.IsNullOrEmpty(result))
        return string.Format("今天");
      return result;
    }
    /// <summary>
    /// 获取司龄
    /// </summary>
    /// <param name="date"></param>
    /// <param name="closedTime"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static string GetZaizhi(DateTime? date, DateTime? closedTime, int status)
    {
      if (date == null)
        return string.Empty;
      var now = status == 0 ? DateTime.Now : closedTime.Value;

      string zaiZhi= DateDiff(date.Value, now);
      if (status == 1 && zaiZhi == "今天")
        zaiZhi = "1天";
      return zaiZhi;
    }
    /// <summary>
    /// 生成员工编号
    /// </summary>
    /// <param name="staffId"></param>
    /// <returns></returns>
    public static string GetStaffNO(int staffId)
    {
      return staffId.ToString("{0:D8}");
    }
  }
}
