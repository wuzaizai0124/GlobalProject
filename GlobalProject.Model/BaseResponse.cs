using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Model
{

  public class BaseResponse
  {
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 返回消息
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }
    public static BaseResponse Ok(string message = "", int code = 0)
    {
      return new BaseResponse { Success = true, Message = message, Code = code };
    }
    public static BaseResponse Error(string message = "", int code = 0)
    {
      return new BaseResponse { Success = false, Message = message, Code = code };
    }
  }
  public class DataResponse<T> : BaseResponse
  {
    /// <summary>
    /// 返回数据
    /// </summary>
    public T Data { get; set; }
    public static DataResponse<T> Ok(T data, string message = "")
    {
      return new DataResponse<T> { Success = true, Message = message, Code = 0, Data = data };
    }
    public static DataResponse<T> Error(string message = "", int code = 0)
    {
      return new DataResponse<T> { Success = false, Message = message, Code = code };
    }
  }
  public class PageResponse<T> : BaseResponse
  {
    /// <summary>
    /// 返回数据
    /// </summary>
    public List<T> Data { get; set; }
    /// <summary>
    /// 总数量
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount { get; set; }


    public static PageResponse<T> Ok(List<T> data, int count, int pageCount = 0, string message = "")
    {
      return new PageResponse<T> { Success = true, Message = message, Code = 0, Data = data, Total = count, PageCount = pageCount };
    }
    public static PageResponse<T> Error(string message = "", int pageCount = 0, int code = 0)
    {
      return new PageResponse<T> { Success = false, Message = message, Code = code, Total = 0, PageCount = pageCount };
    }
  }


}
