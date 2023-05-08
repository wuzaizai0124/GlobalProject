using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure.Utilities
{
  /// <summary>
  /// 功能描述    ：文件帮助类  
  /// 创 建 者    ：admin
  /// 创建日期    ：2022/8/2 15:37:36 
  /// 最后修改者  ：admin
  /// 最后修改日期：2022/8/2 15:37:36 
  /// </summary>
  public static class FileHelper
  {
    private static string ImageExtions = ".png, .jpeg, .gif, .jpg";
    /// 判断文件是否为图片
    /// </summary>
    /// <param name="path">文件的完整路径</param>
    /// <returns>返回结果</returns>
    public static Boolean IsImage(string path)
    {
      try
      {
        if (string.IsNullOrEmpty(path))
          return false;
        if (ImageExtions.Contains(Path.GetExtension(path).ToLower()))
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception e)
      {
        return false;
      }
    }
  }
}
