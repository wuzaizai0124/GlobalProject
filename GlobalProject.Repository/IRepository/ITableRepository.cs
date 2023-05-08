using GlobalProject.Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Repository.IRepository
{
    public interface ITableRepository
    {
        public void CreateModelByTable(string path,string systemName);
    }
}
