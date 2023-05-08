using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public static class AliyunOssExtensions
  {
    public static IServiceCollection AddAliyunOss(this IServiceCollection collection, Action<AliyunOssConfig> config)
    {
      collection.AddSingleton<IAliyunOssService,AliyunOssService>(p=> new AliyunOssService(config));
      return collection;
    }
  }
}
