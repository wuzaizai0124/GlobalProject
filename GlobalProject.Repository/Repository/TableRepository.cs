using GlobalProject.Repository.Core;
using GlobalProject.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalProject.Repository.Repository
{
    public class TableRepository : ITableRepository
    {
        private IUnitOfWork _unitOfWork;
        private SqlSugar.SqlSugarClient _client;
        public TableRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._client = this._unitOfWork.GetDbClient();
        }
        /// <summary>
        /// 根据数据库生成实体
        /// </summary>
        /// <param name="path"></param>
        /// <param name="systemName"></param>
        public void CreateModelByTable(string path, string systemName)
        {
            _client.DbFirst.IsCreateAttribute().FormatFileName(p=>p.ToUpper()).CreateClassFile(path, systemName);
        }
    }
}
