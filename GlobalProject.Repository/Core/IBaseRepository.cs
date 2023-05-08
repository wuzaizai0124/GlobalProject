using GlobalProject.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GlobalProject.Repository.Core
{
  public interface IBaseRepository<T> where T : class
  {
    bool AddEntity(T entity);

    /// <summary>
    /// 添加单个数据,返回主键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    int InsertReturnIdentity(T entity);
    bool AddEntities(IEnumerable<T> entities);
    bool DeleteEntity(T entity);
    //bool DeleteEntities(IEnumerable<T> entities);
    bool UpdateEntity(T entity);

    /// <summary>
    /// 更新指定列数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool UpdateEntityColumns(T entity, Expression<Func<T, object>> where);
    /// <summary>
    /// 更新指定列数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool UpdateEntityColumns(T entity, params string[] columns);
    bool UpdateEntities(IEnumerable<T> entities);
    IEnumerable<T> GetEntities(Expression<Func<T, bool>> where, bool isUserCache = false);
    T GetEntity(Expression<Func<T, bool>> where);
    T GetEntityById(object id);
    bool DeleteEntity(Expression<Func<T, bool>> where);
    bool DeleteEntity(object primaryKey);
    bool DeleteEntities(IEnumerable<object> primaryKeys);

    /// <summary>
    /// 根据sql语句查询集合
    /// </summary>
    /// <param name="strSql">完整的sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>泛型集合</returns>
    public List<T> QuerySql(string strSql, SugarParameter[] parameters = null);

    /// <summary>
    /// 根据sql语句查询单条
    /// </summary>
    /// <param name="strSql">完整的sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>泛型集合</returns>
    public T QuerySqlSingle(string strSql, SugarParameter[] parameters = null);


    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="whereExpression">条件表达式</param>
    /// <param name="intPageIndex">页码（下标0）</param>
    /// <param name="intPageSize">页大小</param>
    /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
    /// <param name="isWithCache">是否使用缓存</param>
    /// <returns></returns>
    public PageResponse<T> QueryPage(Expression<Func<T, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="whereExpression">条件表达式</param>
    /// <param name="intPageIndex">页码（下标0）</param>
    /// <param name="intPageSize">页大小</param>
    /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
    /// <param name="cacheKey">缓存key</param>
    /// <returns></returns>
    public PageResponse<T> QueryPage(Expression<Func<T, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null, string cacheKey = "");
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
    public PageResponse<TResult> QueryTabsPage<T1, T2, TResult>(Expression<Func<T1, T2, object[]>> joinExpression,
        Expression<Func<T1, T2, TResult>> selectExpression,
        Expression<Func<TResult, bool>> whereExpression,
        int intPageIndex = 1,
        int intPageSize = 20,
        string strOrderByFileds = null);


  }
}
