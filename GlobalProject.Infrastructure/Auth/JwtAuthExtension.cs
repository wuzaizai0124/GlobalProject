using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure
{
  public class JwtAuthExtension
  {
    private RequestDelegate _next;
    private string _authUrl;
    public JwtAuthExtension(RequestDelegate next, string authUrl)
    {
      this._next = next;
      _authUrl = authUrl;
    }
    public async Task Invoke(HttpContext context)
    {
      var token = context.Request.Headers["Authorization"].ToString();
      if (string.IsNullOrWhiteSpace(token))
        context.Response.StatusCode = 401;
      var header = new Dictionary<string, object>();
      header.Add("Authorization", token);
      try
      {
        var authCenterResult = HttpHelper.CreatePostHttpResponse(_authUrl, null, null, null, Encoding.UTF8, null, header);
        var result = JsonConvert.DeserializeObject<AuthResponse>(authCenterResult);
        if (!result.Success || result.Code == (int)HttpStatusCode.Unauthorized)
          context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      }
      catch (Exception ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      }
      finally
      {
        if (context.Response.StatusCode != (int)HttpStatusCode.Unauthorized)
          await _next(context);
      }
    }
  }
  public static class JwtAuthExtensionMiddleware
  {
    public static IApplicationBuilder UseRisfondAuht(this IApplicationBuilder builder, string authUrl)
    {
      builder.UseMiddleware<JwtAuthExtension>(new object[1] { authUrl });
      return builder;
    }
  }

}
