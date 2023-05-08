using GlobalProject.Model;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace GlobalProject.Repository.Core
{
  public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new()
  {
    IUnitOfWork _unitOfWork;
    // private IDataBaseFactory _dbFactory;
    public SqlSugarClient _client;
    private SimpleClient<T> _simpleClient;
    public BaseRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
      _client = _unitOfWork.GetDbClient();
      //_dbFactory = dbFactory;
      _simpleClient = new SimpleClient<T>(_client);

#if DEBUG
      _client.Aop.OnLogExecuting = (sql, p) =>
      {
        Array.ForEach(p, o =>
        {
          sql = sql.Replace(o.ParameterName, o.Value?.ToString());
        });
        NLog.LogManager.GetLogger(typeof(BaseRepository<T>).Name).Info(sql);
      };
      _client.Aop.OnLogExecuted = (sql, p) =>
            {
              //执行时间超过1秒
              if (_client.Ado.SqlExecutionTime.TotalSeconds > 0.1)
              {
                //代码CS文件名
                var fileName = _client.Ado.SqlStackTrace.FirstFileName;
                //代码行数
                var fileLine = _client.Ado.SqlStackTrace.FirstLine;
                //方法名
                var FirstMethodName = _client.Ado.SqlStackTrace.FirstMethodName;
                //db.Ado.SqlStackTrace.MyStackTraceList[1].xxx 获取上层方法的信息
              }
              //相当于EF的 PrintToMiniProfiler
            };

#endif
      _client.Aop.OnError = error =>
      {
        Regex reg = new Regex(@"[^0-9]+");
        var sql = error.Sql;
        var pars = error.Parametres as SugarParameter[];
        sql = pars.OrderByDescending(o => reg.Replace(o.ParameterName, "").ObjToInt()).ThenByDescending(o => o.ParameterName.Length).Aggregate(sql, (current, p) => current.Replace(p.ParameterName, (p.DbType == System.Data.DbType.Decimal ||
                                                                                                                                                        p.DbType == System.Data.DbType.Double ||
                                                                                                                                                        p.DbType == System.Data.DbType.Int16 ||
                                                                                                                                                        p.DbType == System.Data.DbType.Int32 ||
                                                                                                                                                        p.DbType == System.Data.DbType.Int64 ||
                                                                                                                                                        p.DbType == System.Data.DbType.Single ||
                                                                                                                                                        p.DbType == System.Data.DbType.VarNumeric ||
                                                                                                                                                        p.DbType == System.Data.DbType.UInt16 ||
                                                                                                                                                        p.DbType == System.Data.DbType.UInt32 ||
                                                                                                                                                        p.DbType == System.Data.DbType.UInt64
            ) ? (p.Value == null || p.Value is System.DBNull) ? "null" : string.Format("{0}", p.Value) : (p.Value == null || p.Value is System.DBNull) ? "null" : string.Format("'{0}'", p.Value)));
        NLog.LogManager.GetLogger(typeof(BaseRepository<T>).Name).Error(sql);
      };
    }

    private ISqlSugarClient _db
    {
      get
      {
        return _client;
      }
    }

    /// <summary>
    ///批量添加数据
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public bool AddEntities(IEnumerable<T> entities)
    {
      return _simpleClient.InsertRange(entities.ToArray());
    }
    /// <summary>
    /// 添加单个数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool AddEntity(T entity)
    {
      return _simpleClient.Insert(entity);
    }

    /// <summary>
    /// 添加单个数据,返回主键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public int InsertReturnIdentity(T entity)
    {
      return _simpleClient.InsertReturnIdentity(entity);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool DeleteEntity(T entity)
    {
      return _simpleClient.Delete(entity);
    }
    /// <summary>
    /// 根据表达式获取数据集合    
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public IEnumerable<T> GetEntities(Expression<Func<T, bool>> where, bool isUserCache = false)
    {
      return _simpleClient.AsQueryable().Where(where).WithCacheIF(isUserCache).ToList();
    }
    /// <summary>
    /// 根据条件获取单个实体数据
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public T GetEntity(Expression<Func<T, bool>> where)
    {
      return _simpleClient.AsQueryable().Where(where).First();
    }
    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetEntityById(object id)
    {
      return _simpleClient.GetById(id);
    }
    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public bool UpdateEntities(IEnumerable<T> entities)
    {
      return _simpleClient.UpdateRange(entities.ToArray());
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool UpdateEntity(T entity)
    {
      return _simpleClient.Update(entity);
    }

    /// <summary>
    /// 更新指定列数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool UpdateEntityColumns(T entity, Expression<Func<T, object>> where)
    {
      var result = _client.Updateable(entity).UpdateColumns(where).ExecuteCommand();
      return result > 0 ? true : false;
    }
    /// <summary>
    /// 更新指定列数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool UpdateEntityColumns(T entity, params string[] columns)
    {
      var result = _client.Updateable(entity).UpdateColumns(columns).ExecuteCommand();
      return result > 0 ? true : false;
    }
    /// <summary>
    /// 根据表达式删除实体
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public bool DeleteEntity(Expression<Func<T, bool>> where)
    {
      return _client.Deleteable<T>().Where(where).ExecuteCommand() > 0;
    }
    /// <summary>
    /// 根据主键删除数据
    /// </summary>
    /// <param name="primaryKey"></param>
    /// <returns></returns>
    public bool DeleteEntity(object primaryKey)
    {
      return _simpleClient.DeleteById(primaryKey);
    }
    /// <summary>
    /// 根据主键集合删除数据
    /// </summary>
    /// <param name="primaryKeys"></param>
    /// <returns></returns>
    public bool DeleteEntities(IEnumerable<object> primaryKeys)
    {
      return _simpleClient.DeleteByIds(primaryKeys.ToArray());
    }

    /// <summary>
    /// 根据sql语句查询集合
    /// </summary>
    /// <param name="strSql">完整的sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>泛型集合</returns>
    public List<T> QuerySql(string strSql, SugarParameter[] parameters = null)
    {
      return _client.Ado.SqlQuery<T>(strSql, parameters);
    }

    /// <summary>
    /// 根据sql语句查询单条
    /// </summary>
    /// <param name="strSql">完整的sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>泛型集合</returns>
    public T QuerySqlSingle(string strSql, SugarParameter[] parameters = null)
    {
      return _client.Ado.SqlQuerySingle<T>(strSql, parameters);
    }


    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="whereExpression">条件表达式</param>
    /// <param name="intPageIndex">页码（下标0）</param>
    /// <param name="intPageSize">页大小</param>
    /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
    /// <param name="isWithCache">是否使用缓存</param>
    /// <returns></returns>
    public PageResponse<T> QueryPage(Expression<Func<T, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
    {
      if (intPageSize <= 0)
        intPageSize = 20;

      int totalCount = 0;
      var list = _client.Queryable<T>()
       .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
       .WhereIF(whereExpression != null, whereExpression)
       .ToPageList(intPageIndex, intPageSize, ref totalCount);

      int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
      return PageResponse<T>.Ok(list, totalCount, pageCount);
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="whereExpression">条件表达式</param>
    /// <param name="intPageIndex">页码（下标0）</param>
    /// <param name="intPageSize">页大小</param>
    /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
    /// <param name="cacheKey">缓存key</param>
    /// <returns></returns>
    public PageResponse<T> QueryPage(Expression<Func<T, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null, string cacheKey = "")
    {
      if (intPageSize <= 0)
        intPageSize = 20;

      int totalCount = 0;
      var list = _client.Queryable<T>()
       .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
       .WhereIF(whereExpression != null, whereExpression)
       .WithCache(cacheKey)
       .ToPageList(intPageIndex, intPageSize, ref totalCount);

      int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
      return PageResponse<T>.Ok(list, totalCount, pageCount);
    }
    /// <summary>
    /// 两表联合查询-分页
    /// </summary>
    /// <typeparam name="T1">实体1</typeparam>
    /// <typeparam name="T2">实体1</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="joinExpression">关联表达式</param>
    /// <param name="selectExpression">返回表达式</param>
    /// <param name="whereExpression">查询表达式</param>
    /// <param name="intPageIndex">页码</param>
    /// <param name="intPageSize">页大小</param>
    /// <param name="strOrderByFileds">排序字段</param>
    /// <returns></returns>
    public PageResponse<TResult> QueryTabsPage<T1, T2, TResult>(
        Expression<Func<T1, T2, object[]>> joinExpression,
        Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<TResult, bool>> whereExpression,
        int intPageIndex = 1,
        int intPageSize = 20,
        string strOrderByFileds = null)
    {

      int totalCount = 0;
      var list = _client.Queryable<T1, T2>(joinExpression)
       .Select(selectExpression)
       .MergeTable()
       .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
       .WhereIF(whereExpression != null, whereExpression)
       .ToPageList(intPageIndex, intPageSize, ref totalCount);

      int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
      return PageResponse<TResult>.Ok(list, totalCount, pageCount);
    }


  }
}
