using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using GlobalProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalProject.Api.Middleware
{
  public class ValidateModelAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        var result = context.ModelState.Keys
                .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();

        string message = null;
        if (result != null)
        {
          foreach (var item in result)
          {
            message += item.Message + "，";
          }
        }
        context.Result = new ObjectResult(DataResponse<string>.Error(message));
      }
    }
  }
  public class ValidationError
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Field { get; }
    public string Message { get; }
    public ValidationError(string field, string message)
    {
      Field = field != string.Empty ? field : null;
      Message = message;
    }
  }
}