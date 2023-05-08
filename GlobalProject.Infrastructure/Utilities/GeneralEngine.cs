using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public class GeneralEngine : IEngine
  {
    private IServiceProvider _provider;
    public GeneralEngine(IServiceProvider provider)
    {
      this._provider = provider;
    }
    public T Resolve<T>() //where T: class
    {
      return (T)_provider.GetService(typeof(T));
    }
  }
}
