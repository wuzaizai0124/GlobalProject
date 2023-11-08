using GlobalProject.Infrastructure.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
//using Microsoft.Extensions.Logging;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestElkController : ControllerBase
    {
        //NLogHelper<TestElkController> _logHelper;
        ILogger nlog = LogManager.GetLogger("Nlog");
        //private ILogger _logger;
        public TestElkController()
        {
            //this._logger = logger;
            //_logHelper = new NLogHelper<TestElkController>(logger);
        }
        [HttpGet]
        [Route("test")]        
        public string Test(string name)
        {
            //_logger.Log(LogLevel.Information,"测试");
            //throw new Exception("测试错误");
            //_logHelper.WriteLog(LogLevel.Error, "测试异常");
            //_logHelper.WriteLog(LogLevel.Info, "测试异常");
            nlog.Info("测试数据");
            nlog.Error("测试异常");
            nlog.Debug("测试断点");
            var nameInt = Convert.ToInt32(name);
            return name;
        }
    }
}
