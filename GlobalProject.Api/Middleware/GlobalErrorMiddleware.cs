using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalProject.Api.Middleware
{
    public class GlobalErrorMiddleware
    {
        private RequestDelegate _next;
        public GlobalErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
               await _next(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
    public static class MiddlewareExtension
    {
        /// <summary>
        /// 自定义全局错误捕获
        /// </summary>
        /// <param name="app"></param>
        public static void UseGolobalException(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(GlobalErrorMiddleware));
        }
    }
}
