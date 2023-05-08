using Microsoft.Extensions.Configuration;
using GlobalProject.Infrastructure;
using GlobalProject.Infrastructure.Utilities;
using GlobalProject.Repository.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Repository.Core
{
  public class DataBaseFactory : IDataBaseFactory, IDisposable
  {
    private SqlSugarClient _sqlSugarClient;

    public void Dispose()
    {
      if (_sqlSugarClient != null)
        _sqlSugarClient.Dispose();
    }
    public SqlSugarClient GetDbContext()
    {
      return _sqlSugarClient ?? GetDbClient();
    }
    /// <summary>
    /// 实例化数据库访问上下文
    /// </summary>
    /// <returns></returns>
    private SqlSugarClient GetDbClient()
    {
      var configuration = EngineContext.Current.Resolve<IConfiguration>();
      var config = new SqlSugarConfig();
      configuration.GetSection("SqlSugarConfig").Bind(config);
      var dbClient = new SqlSugarClient(new ConnectionConfig
      {
        ConnectionString = config.ConnectionString,
        DbType = (DbType)config.DbType,
        InitKeyType = (InitKeyType)config.InitKeyType,
        IsAutoCloseConnection = config.IsAutoCloseConnection,        
        //全自动清理：
        //所有 增、删 、改 会自动调用.RemoveDataCache()
        MoreSettings = new ConnMoreSettings()
        {
          IsAutoRemoveDataCache = true
        }
        //缺点：
        //基本单表查询和联表查询都支持，如果用到子查询或者Mergetable就无法自动清除了，这种情况用 【4.2追加key模式】
      }); ;;
      return dbClient;
    }
  }
}
