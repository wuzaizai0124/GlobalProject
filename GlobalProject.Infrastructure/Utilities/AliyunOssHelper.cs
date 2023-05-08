using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.IO;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public class AliyunOssConfig
  {
    public string EndPoint { get; set; }
    public string AccessKey { get; set; }
    public string AccessSecret { get; set; }
    public string Bucket { get; set; }
    public string UrlForView { get; set; }
    /// <summary>
    /// 文件夹路径
    /// </summary>
    public string FileUrl { get; set; } = "";
  }

  public class AliyunOssHelper
  {
    private AliyunOssConfig _oss;

    public AliyunOssHelper(Action<AliyunOssConfig> config)
    {
      _oss = new AliyunOssConfig();
      config(_oss);
    }
    public static class FileType
    {
      /// <summary>
      /// hr-img
      /// </summary>
      public const int UserHeadImg = 1;

      /// <summary>
      /// 企业logo
      /// </summary>
      public const int CompanyLogoImg = 2;

      /// <summary>
      /// 公司章
      /// </summary>
      public const int Signature = 3;

      /// <summary>
      /// 员工合同
      /// </summary>
      public const int StaffContract = 4;

      /// <summary>
      /// 签约列表
      /// </summary>
      public const int StaffSign = 5;
    }


    public string GetPath(int fileType, string fileName)
    {
      DateTime now = DateTime.Now;
      string path = string.Empty;
      Random random = new Random();
      string ran = random.Next(100000, 999999).ToString();
      switch (fileType)
      {
        case FileType.UserHeadImg:
          path = $"hr-img/{now:yyyy}/{now:yyyyMM}/{now:yyyyMMdd}/{now:yyyyMMdd_HHmmssfff}" + ran +"_"+ fileName;
          break;
        case FileType.CompanyLogoImg:
          path = $"logo-img/{now:yyyy}/{now:yyyyMM}/{now:yyyyMMdd}/{now:yyyyMMdd_HHmmssfff}" + ran + "_" + fileName;
          break;
        case FileType.Signature:
          path = $"Signature/{now:yyyy}/{now:yyyyMM}/{now:yyyyMMdd}/{now:yyyyMMdd_HHmmssfff}" + ran + "_" + fileName;
          break;
        case FileType.StaffContract:
          path = $"StaffContract/{now:yyyy}/{now:yyyyMM}/{now:yyyyMMdd}/{now:yyyyMMdd_HHmmssfff}" + ran + "_" + fileName;
          break;
        case FileType.StaffSign:
          path = $"StaffSign/{now:yyyy}/{now:yyyyMM}/{now:yyyyMMdd}/{now:yyyyMMdd_HHmmssfff}" + ran + "_" + fileName;
          break;
        default:

          break;
      }
      return "hrsop/" + _oss.FileUrl + path;
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <returns></returns>
    public bool UploadFile(byte[] imgBytes, string fileName, int fileType, out string key, out string fullPath)
    {
      key = string.Empty;
      fullPath = string.Empty;
      try
      {
        var now = DateTime.Now;
        MemoryStream fileStream = new MemoryStream(imgBytes);
        key = GetPath(fileType, fileName);
        if (string.IsNullOrEmpty(key))
        {
          return false;
        }
        var oss = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
        var putResult = oss.PutObject(_oss.Bucket, key, fileStream);
        oss.SetObjectAcl(_oss.Bucket, key, CannedAccessControlList.Private);
        fullPath = string.Concat(_oss.UrlForView, "/", key);
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }


    public bool UploadPublicFile(byte[] imgBytes, string fileName, int fileType, out string key, out string fullPath)
    {
      key = string.Empty;
      fullPath = string.Empty;
      try
      {
        var now = DateTime.Now;
        MemoryStream fileStream = new MemoryStream(imgBytes);
        key = GetPath(fileType, fileName);
        if (string.IsNullOrEmpty(key))
        {
          return false;
        }
        var oss = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
        var putResult = oss.PutObject(_oss.Bucket, key, fileStream);
        oss.SetObjectAcl(_oss.Bucket, key, CannedAccessControlList.PublicRead);
        fullPath = $"{_oss.UrlForView}/{key}";
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool DeleteFile(string key)
    {
      try
      {
        var now = DateTime.Now;
        var oss = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
        oss.DeleteObject(_oss.Bucket, key);
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }

    /// <summary>
    /// 取得文件临时访问URL，URL过期时间1小时（自定义）
    /// </summary>
    /// <param name="fileKey"></param>
    /// <returns></returns>
    public string GetTempVisitUrl(string fileKey)
    {
      if (string.IsNullOrEmpty(fileKey))
      {
        return string.Empty;
      }
      var ossClient = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
      var req = new GeneratePresignedUriRequest(_oss.Bucket, fileKey, SignHttpMethod.Get)
      {
        Expiration = DateTime.Now.AddHours(1)
      };
      var uri = ossClient.GeneratePresignedUri(req);
      if (uri == null)
      {
        return string.Empty;
      }
      var url = _oss.UrlForView + uri.PathAndQuery;
      return url;
    }
    /// <summary>
    /// 取得文件临时访问URL，URL过期时间1小时（自定义）
    /// </summary>
    /// <param name="fileKey"></param>
    /// <returns></returns>
    public string GetTempVisitUrl(string fileKey, bool isWebp)
    {
      if (string.IsNullOrEmpty(fileKey))
      {
        return string.Empty;
      }
      var ossClient = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
      var req = new GeneratePresignedUriRequest(_oss.Bucket, fileKey, SignHttpMethod.Get)
      {
        Expiration = DateTime.Now.AddHours(1)
      };
      if (isWebp)
      {
        req.Process = "image/resize,p_100/format,webp";
      }
      var uri = ossClient.GeneratePresignedUri(req);
      if (uri == null)
      {
        return string.Empty;
      }
      var url = _oss.UrlForView + uri.PathAndQuery;
      return url;
    }
    /// <summary>
    /// 取得文件临时访问URL，URL过期时间1小时（自定义）
    /// </summary>
    /// <param name="fileKey"></param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="isWebp">是否是webp格式</param>
    /// <param name="firstType">缩放方式，0：无，1：短边优先，2：长边优先</param>
    /// <returns></returns>
    public string GetTempVisitUrl(string fileKey, int? width, int? height, bool isWebp, int firstType)
    {
      if (string.IsNullOrEmpty(fileKey))
      {
        return string.Empty;
      }
      var ossClient = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
      var req = new GeneratePresignedUriRequest(_oss.Bucket, fileKey, SignHttpMethod.Get)
      {
        Expiration = DateTime.Now.AddHours(1)
      };
      StringBuilder proBuilder = new StringBuilder();
      proBuilder.Append("image/resize");
      if (width.HasValue || height.HasValue)
      {
        if (firstType == 1)
          proBuilder.Append(",m_mfit");
        if (firstType == 2)
          proBuilder.Append(",m_lfit");
      }
      if (width.HasValue)
        proBuilder.AppendFormat(",h_{0}", width.Value);
      if (height.HasValue)
        proBuilder.AppendFormat(",w_{0}", height);
      if (!width.HasValue && !height.HasValue)
        proBuilder.Append("p_100");
      if (isWebp)
        proBuilder.Append("/format,webp");
      req.Process = proBuilder.ToString();
      var uri = ossClient.GeneratePresignedUri(req);
      if (uri == null)
      {
        return string.Empty;
      }
      var url = _oss.UrlForView + uri.PathAndQuery;
      return url;
    }

    /// <summary>
    /// base64解码文件流
    /// </summary>
    /// <returns></returns>
    public static byte[] GetFileBytes(string base64File)
    {
      if (string.IsNullOrEmpty(base64File))
        return null;
      var strArray = base64File.Split(',');
      string strFile = string.Empty;
      if (strArray.Length == 2)
      {
        strFile = strArray[1];
      }
      else
      {
        strFile = base64File;
      }
      try
      {
        return Convert.FromBase64String(strFile);
      }
      catch (Exception e)
      {
        return null;
      }

    }
    /// <summary>
    /// 取得定制大小的图片文件(公共资源可用)
    /// </summary>
    /// <param name="fileKey"></param>
    /// <param name="height"></param>
    /// <param name="wight"></param>
    /// <returns></returns>
    public string CustomizationPublicImgUrl(string fileKey, int height, int wight, bool isiOS)
    {
      if (string.IsNullOrEmpty(fileKey))
        return null;
      string result = string.Empty;
      result = _oss.UrlForView + "/" + fileKey + "?x-oss-process=image/resize,m_fixed,h_" + height + ",w_" + wight;
      if (!isiOS)
      {
        result += "/format,webp";
      }
      return result;
    }
    /// <summary>
    /// 下载下载阿里文件
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public byte[] GetBytes(string length)
    {
      var ossClient = new OssClient(_oss.EndPoint, _oss.AccessKey, _oss.AccessSecret);
      var ossFile = ossClient.GetObject(_oss.Bucket, length);
      var content = new byte[ossFile.ContentLength];
      var readCount = 0;
      var offset = 0;
      do
      {
        readCount = ossFile.Content.Read(content, offset, content.Length - offset);
        offset += readCount;
      } while (readCount > 0);

      return content;
    }
  }
}
