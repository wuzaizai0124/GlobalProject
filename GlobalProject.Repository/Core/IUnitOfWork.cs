using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GlobalProject.Repository.Core
{
  public interface IUnitOfWork
  {
    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
  public  SqlSugarClient GetDbClient();
    void BeginTran(IsolationLevel isolationLevel);
    void BeginTran();
    void CommitTran();
    void RollBackTran();
  }
}
