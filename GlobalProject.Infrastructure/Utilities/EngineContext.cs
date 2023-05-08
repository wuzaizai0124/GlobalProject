using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GlobalProject.Infrastructure
{
  public interface IEngine
  {
    T Resolve<T>();
  }
  public class EngineContext
  {
    private static IEngine _engine;
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static IEngine initialize(IEngine engine)
    {
      if (_engine == null)
      {
        _engine = engine;
      }
      return _engine;
    }
    /// <summary>
    /// 当前引擎
    /// </summary>
    public static IEngine Current
    {
      get
      {
        return _engine;
      }
    }
  }
}
