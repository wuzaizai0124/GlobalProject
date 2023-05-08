using GlobalProject.Repository.IRepository;
using GlobalProject.Services.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Services.Service
{
    public class TableService:ITableService
    {
        private ITableRepository _tableRepository;
        public TableService(ITableRepository tableRepository)
        {
            this._tableRepository = tableRepository;
        }

        public void CreateModelByTable(string path, string systemName)
        {
            this._tableRepository.CreateModelByTable(path,systemName);
        }
    }
}
