using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure.Utilities
{
  /// <summary>
  /// IHttpContextAccessorExtensions
  /// </summary>
  public static class IHttpContextAccessorExtensions
  {
    /// <summary>
    /// GetValue
    /// </summary>
    /// <param name="httpContext">httpContext</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    public static string GetValue(this IHttpContextAccessor httpContext, string key)
    {
      if (httpContext.HttpContext.Items.Any() && httpContext.HttpContext.Items.ContainsKey(key))
      {
        var item = httpContext.HttpContext.Items.First((KeyValuePair<object, object> o) => o.Key.Equals(key));
        return item.Value?.ToString();
      }

      return string.Empty;
    }
  }
}
