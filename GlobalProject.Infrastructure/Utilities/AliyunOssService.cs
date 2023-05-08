using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public class AliyunOssService: IAliyunOssService
  {
    private Action<AliyunOssConfig> _config;
    private AliyunOssHelper _ossHelper;
    public AliyunOssService(Action<AliyunOssConfig> config)
    {
      _config = config;
    }
    private AliyunOssHelper GetAliyunClient()
    {
      this._ossHelper = new AliyunOssHelper(_config);
      return this._ossHelper;
    }
    public AliyunOssHelper Current
    {
      get { return _ossHelper ?? GetAliyunClient(); }
    }
    
  }
}
