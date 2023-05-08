using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace GlobalProject.Infrastructure.Log
{
    public class NlogMiddlewareProvider
    {
        private readonly RequestDelegate _next;
        private ILogger _logger;
        private NLogHelper<ControllerBase> _logHelper;
        public NlogMiddlewareProvider(RequestDelegate next, string config)
        {
            this._next = next;
            LogManager.LoadConfiguration(config);
            _logger = LogManager.GetCurrentClassLogger();
            _logHelper = new NLogHelper<ControllerBase>(_logger);
        }
        public async Task Invoke(HttpContext context)
        {
            string requestUrl = string.Empty;
            string requestDataStr = string.Empty;
            string responseDataStr = string.Empty;
            var logLevel = LogLevel.Info;
            try
            {
                context.Request.EnableBuffering();
                context.Request.Body.Seek(0, 0);
                _logHelper.StartTime = DateTime.Now;
                requestUrl = context.Request.Path;
                if (context.Request.ContentLength > 0)
                {
                    using (var buffer = new MemoryStream())
                    {
                        await context.Request.Body.CopyToAsync(buffer);
                        buffer.Position = 0;
                        requestDataStr = System.Text.Encoding.Default.GetString(buffer.ToArray());
                    }
                }
                context.Request.Body.Position = 0;
                var responseOriginalBody = context.Response.Body;
                using var memStream = new MemoryStream();
                context.Response.Body = memStream;
                await _next(context);               
                responseDataStr = await GetResponse(context.Response);                
                await memStream.CopyToAsync(responseOriginalBody);
                context.Response.Body = responseOriginalBody;
            }
            catch (Exception ex)
            {                
                _logHelper.Exception = ex;// exception.Error;
                logLevel = LogLevel.Error;
            }
            finally
            {
                // 响应完成记录时间和存入日志
                context.Response.OnCompleted(() =>
                {
                    _logHelper.EndTime = DateTime.Now;
                    _logHelper.WriteLog(logLevel, requestDataStr, responseDataStr, requestUrl);
                    return Task.CompletedTask;
                });
            }
        }
        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseNlog(this IApplicationBuilder builder, string config)
        {
            return builder.UseMiddleware<NlogMiddlewareProvider>(new object[1] { config });
        }
    }
}
