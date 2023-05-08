using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GlobalProject.Repository.Core
{
  public class UnitOfWork : IUnitOfWork, IDisposable
  {
    private IDataBaseFactory _dbFactory;
    private SqlSugarClient _client;
    public UnitOfWork(IDataBaseFactory factory)
    {
      this._dbFactory = factory;
      _client = _dbFactory.GetDbContext();
    }


    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    public SqlSugarClient GetDbClient()
    {
      // 必须要as，后边会用到切换数据库操作
      return _client;
    }


    /// <summary>
    /// 开启事务
    /// </summary>
    public void BeginTran(IsolationLevel isolationLevel)
    {
      _client.Ado.BeginTran(isolationLevel);
    }
    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTran()
    {
      _client.CommitTran();
    }
    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollBackTran()
    {
      _client.RollbackTran();
    }
    /// <summary>
    /// 释放链接
    /// </summary>
    public void Dispose()
    {
      if (_client != null)
        _client.Dispose();
    }
    /// <summary>
    /// 开启事务
    /// </summary>
    public void BeginTran()
    {
      _client.Ado.BeginTran();
    }
  }
}
