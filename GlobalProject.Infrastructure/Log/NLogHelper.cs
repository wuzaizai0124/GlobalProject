using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure.Log
{
  public class NLogHelper<T>
  {
    private ILogger _logger;
    public Exception Exception { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    private IConfiguration _config;
    public NLogHelper(ILogger logger)
    {
      _logger = logger;
    }
    /// <summary>
    /// 记录日志
    /// </summary>
    /// <typeparam name="T">类名称</typeparam>
    /// <param name="level">日至等级</param>
    /// <param name="requestData">接受参数</param>
    /// <param name="resultData">返回参数</param>
    /// <param name="funcName">方法名</param>        
    public void WriteLog(LogLevel level, object requestData, object resultData, string funcName)
    {
      //异步记录日志         
      StringBuilder logInfo = new StringBuilder();
      logInfo.Append("\r\n*************************************开始***********************************\r\n");
      logInfo.AppendFormat("\r\n【记录时间】：{0}\r\n", DateTime.Now.ToString(CultureInfo.InvariantCulture));
      if (StartTime.HasValue)
        logInfo.AppendFormat("【请求开使时间】:{0}\r\n", StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff"));
      logInfo.AppendFormat("【调用Contrroler】:{0}\r\n", typeof(T));
      logInfo.AppendFormat("【调用方法】：{0}\r\n", funcName);
      logInfo.AppendFormat("【请求数据】：{0}\r\n", JsonConvert.SerializeObject(requestData));
      logInfo.AppendFormat("【返回数据】：{0}\r\n", JsonConvert.SerializeObject(resultData));
      if (Exception != null)
      {
        logInfo.AppendFormat("【错误信息】：{0}\r\n", Exception.Message);
        logInfo.AppendFormat("【错误源】：{0}\r\n", Exception.Source);
        logInfo.AppendFormat("【引发异常方法】：{0}\r\n", Exception.TargetSite);
        logInfo.AppendFormat("【详情信息】：{0}\r\n", Exception.StackTrace);
      }
      if (EndTime.HasValue)
        logInfo.AppendFormat("【请求结束时间】:{0}\r\n", EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff"));
      logInfo.Append("\r\n*************************************结束***********************************\r\n");
      _logger.Log(level, logInfo.ToString);
    }
    public void WriteLog(LogLevel level, string requestData, string resultData, string requestPath)
    {
      //异步记录日志         
      StringBuilder logInfo = new StringBuilder();
      logInfo.Append("\r\n*************************************开始***********************************\r\n");
      logInfo.AppendFormat("\r\n【记录时间】：{0}\r\n", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
      if (StartTime.HasValue)
        logInfo.AppendFormat("【请求开使时间】:{0}\r\n", StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff"));
      logInfo.AppendFormat("【调用路径】：{0}\r\n", requestPath);
      logInfo.AppendFormat("【请求数据】：{0}\r\n", requestData);
      logInfo.AppendFormat("【返回数据】：{0}\r\n", resultData);
      if (Exception != null)
      {
        logInfo.AppendFormat("【错误信息】：{0}\r\n", Exception.Message);
        logInfo.AppendFormat("【错误源】：{0}\r\n", Exception.Source);
        logInfo.AppendFormat("【引发异常方法】：{0}\r\n", Exception.TargetSite);
        logInfo.AppendFormat("【详情信息】：{0}\r\n", Exception.StackTrace);
      }
      if (EndTime.HasValue)
        logInfo.AppendFormat("【请求结束时间】:{0}\r\n", EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff"));
      logInfo.Append("\r\n*************************************结束***********************************\r\n");
      _logger.Log(level, logInfo.ToString);
    }
    public void WriteLog(LogLevel level, string requestData)
    {
      StringBuilder logInfo = new StringBuilder();
      logInfo.Append("\r\n*************************************开始***********************************\r\n");
      logInfo.AppendFormat("\r\n{0}\r\n", requestData);
      if (Exception != null)
      {
        logInfo.AppendFormat("【错误信息】：{0}\r\n", Exception.Message);
        logInfo.AppendFormat("【错误源】：{0}\r\n", Exception.Source);
        logInfo.AppendFormat("【引发异常方法】：{0}\r\n", Exception.TargetSite);
        logInfo.AppendFormat("【详情信息】：{0}\r\n", Exception.StackTrace);
      }
      logInfo.Append("\r\n*************************************结束***********************************\r\n");
      _logger.Log(level, logInfo.ToString());
    }
  }
}
