﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace GlobalProject.Infrastructure
{
  /// <summary>
  /// AutoMapper扩展
  /// </summary>
  public static class AutoMapperExtension
  {
    /// <summary>
    /// 类型映射
    /// </summary>
    /// <typeparam name="TDestination">映射后的对象</typeparam>
    /// <param name="obj">要映射的对象</param>
    /// <returns></returns>
    public static TDestination MapTo<TDestination>(this TDestination obj) where TDestination : class
    {
      if (obj == null) return default(TDestination);
      var config = new MapperConfiguration(cfg => cfg.CreateMap<TDestination, object>());
      var mapper = config.CreateMapper();
      return mapper.Map<TDestination>(obj);
    }
        /// <summary>
        /// 类型映射
        /// </summary>
        /// <typeparam name="TDestination">映射后的对象</typeparam>
        /// <param name="obj">要映射的对象</param>
        /// <returns></returns>
        public static TDestination MapTo<TSource,TDestination>(this TSource obj) where TDestination : class
        {
            if (obj == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TDestination, object>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(obj);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<TDestination> MapTo<TDestination>(this IEnumerable<TDestination> source) where TDestination : class
    {
      if (source == null) return default(List<TDestination>);
      var config = new MapperConfiguration(cfg => cfg.CreateMap(source.GetType(), typeof(TDestination)));
      var mapper = config.CreateMapper();
      return mapper.Map<List<TDestination>>(source);
    }

    /// <summary>
    /// 集合列表类型映射
    /// </summary>
    /// <typeparam name="TSource">数据源类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">数据源</param>
    /// <returns></returns>
    public static List<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source)
        where TDestination : class
        where TSource : class
    {
      if (source == null) return new List<TDestination>();

      var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
      var mapper = config.CreateMapper();
      return mapper.Map<List<TDestination>>(source);
    }

    /// <summary>
    /// 集合列表类型映射
    /// </summary>
    /// <typeparam name="TSource">数据源类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="configure">自定义配置</param>
    /// <returns></returns>
    public static List<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source, Action<IMapperConfigurationExpression> configure)
        where TDestination : class
        where TSource : class
    {
      if (source == null) return new List<TDestination>();

      var config = new MapperConfiguration(configure);
      var mapper = config.CreateMapper();
      return mapper.Map<List<TDestination>>(source);
    }

    /// <summary>
    /// 类型映射
    /// </summary>
    /// <typeparam name="TSource">数据源类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="destination">目标对象</param>
    /// <returns></returns>
    public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        where TSource : class
        where TDestination : class
    {
      if (source == null) return destination;

      var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
      var mapper = config.CreateMapper();
      return mapper.Map<TSource, TDestination>(source, destination);
    }

    /// <summary>
    /// 类型映射,默认字段名字一一对应
    /// </summary>
    /// <typeparam name="TDestination">转化之后的model，可以理解为viewmodel</typeparam>
    /// <typeparam name="TSource">要被转化的实体，Entity</typeparam>
    /// <param name="source">可以使用这个扩展方法的类型，任何引用类型</param>
    /// <returns>转化之后的实体</returns>
    public static TDestination MapTo<TSource, TDestination>(this TSource source, bool ignore = false)
        where TDestination : class
        where TSource : class
    {
      if (source == null) return default(TDestination);
      var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>().IgnoreAllNonAttributedEntities(ignore));
      var mapper = config.CreateMapper();

      return mapper.Map<TDestination>(source);
    }
    public static IMappingExpression<TSource, TDestination> IgnoreAllNonAttributedEntities<TSource, TDestination>
    (this IMappingExpression<TSource, TDestination> expression, bool ignore)
    {
      if (ignore)
      {
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var sourceType = typeof(TSource);
        foreach (var prop in sourceType.GetProperties(flags))
        {
          var attr = prop.GetCustomAttribute(typeof(IgnoreAttribute), false);
          if (attr != null)
          {
            expression.ForMember(prop.Name, opt => opt.Ignore());
          }
        }
        var destinationType = typeof(TDestination);
        foreach (var prop in destinationType.GetProperties(flags))
        {
          var attr = prop.GetCustomAttribute(typeof(IgnoreAttribute), false);
          if (attr != null)
          {
            expression.ForMember(prop.Name, opt => opt.Ignore());
          }
        }
      }
      return expression;
    }
    public static TDestination MapTo<TSource, TDestination>(this TSource source, MapperConfiguration cfg)
      where TDestination : class
      where TSource : class
    {
      if (source == null) return default(TDestination);
      var config = cfg; //new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
      var mapper = config.CreateMapper();
      return mapper.Map<TDestination>(source);
    }
    /// <summary>
    /// 集合列表类型映射
    /// </summary>
    /// <typeparam name="TSource">数据源类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="configure">自定义配置</param>
    /// <returns></returns>
    public static List<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source, MapperConfiguration config)
        where TDestination : class
        where TSource : class
    {
      if (source == null) return new List<TDestination>();

      var _config = config;
      var mapper = _config.CreateMapper();
      return mapper.Map<List<TDestination>>(source);
    }
  }
}
