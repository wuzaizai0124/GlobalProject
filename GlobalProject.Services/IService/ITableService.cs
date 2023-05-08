using GlobalProject.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Services.IService
{
    public interface ITableService
    {
        public void CreateModelByTable(string path, string systemName);        
    }
}
