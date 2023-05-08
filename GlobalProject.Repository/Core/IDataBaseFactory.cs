using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Repository.Core
{
  public interface IDataBaseFactory
  {
    SqlSugarClient GetDbContext();
  }
}
