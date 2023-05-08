using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public interface IAliyunOssService
  {
    AliyunOssHelper Current { get;  }
  }
}
