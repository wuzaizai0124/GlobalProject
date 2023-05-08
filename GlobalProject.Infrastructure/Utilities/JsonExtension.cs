using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlobalProject
{
  public static class JsonExtension
  {
    public static object ToJson(this string Json)
    {
      return Json == null ? null : JsonConvert.DeserializeObject(Json);
    }
    public static string ToJson(this object obj, bool isIgnoreNullValue = false)
    {
      var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
      JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
      jsonSerializerSettings.Converters.Add(timeConverter);
      if (isIgnoreNullValue)
      {
        jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
      }
      return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
    }
    public static string ToJson(this object obj, string datetimeformats)
    {
      var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
      return JsonConvert.SerializeObject(obj, timeConverter);
    }
    public static T ToObject<T>(this string Json)
    {
      return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
    }
    public static List<T> ToList<T>(this string Json)
    {
      return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
    }
    public static DataTable ToTable(this string Json)
    {
      return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
    }
    public static JObject ToJObject(this string Json)
    {
      return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
    }
    public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
    {
      // 定义集合 
      List<T> ts = new List<T>();
      //定义一个临时变量 
      string tempName = string.Empty;
      //遍历DataTable中所有的数据行 
      foreach (DataRow dr in dt.Rows)
      {
        T t = new T();
        // 获得此模型的公共属性 
        PropertyInfo[] propertys = t.GetType().GetProperties();
        //遍历该对象的所有属性 
        foreach (PropertyInfo pi in propertys)
        {
          tempName = pi.Name;//将属性名称赋值给临时变量 
                             //检查DataTable是否包含此列（列名==对象的属性名）  
          if (dt.Columns.Contains(tempName))
          {
            //取值 
            object value = dr[tempName];
            //如果非空，则赋给对象的属性 
            if (value != DBNull.Value)
            {
              pi.SetValue(t, value, null);
            }
          }
        }
        //对象添加到泛型集合中 
        ts.Add(t);
      }
      return ts;
    }
    public static T GetEntity<T>(DataTable table) where T : new()
    {
      T entity = new T();
      foreach (DataRow row in table.Rows)
      {
        foreach (var item in entity.GetType().GetProperties())
        {
          if (row.Table.Columns.Contains(item.Name))
          {
            if (DBNull.Value != row[item.Name] && row[item.Name] != null)
            {
              Type type = row[item.Name].GetType();

              //判断type类型是否为泛型，因为nullable是泛型类,
              if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
              {
                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(type);
                //将type转换为nullable对的基础基元类型
                type = nullableConverter.UnderlyingType;
                item.SetValue(entity, Convert.ChangeType(row[item.Name], type), null);
              }
              item.SetValue(entity, Convert.ChangeType(row[item.Name], type), null);

            }

          }
        }
      }

      return entity;
    }
  }
}
