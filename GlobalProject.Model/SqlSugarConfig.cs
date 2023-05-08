using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Repository.Model
{
  public class SqlSugarConfig
  {
    /// <summary>
    /// 链接字符串
    /// </summary>
    public string ConnectionString { get; set; }
    /// <summary>
    /// 数据库类型
    /// MySql = 0,
    ///SqlServer = 1,
    ///Sqlite = 2,
    ///Oracle = 3,
    ///PostgreSQL = 4
    /// </summary>
    public int DbType { get; set; }
    /// <summary>
    /// //自动释放数据务，如果存在事务，在事务结束后释放
    /// </summary>
    public bool IsAutoCloseConnection { get; set; }
    /// <summary>
    /// 从实体特性中读取主键自增列信息
    /// </summary>
    public int InitKeyType { get; set; }
  }
}
