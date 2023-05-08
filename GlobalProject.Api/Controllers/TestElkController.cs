using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        //private ILogger _logger;
        public TestElkController()
        {
            //this._logger = logger;
        }
        [HttpGet]
        [Route("test")]
        public string Test(string name)
        {
            //_logger.Log(LogLevel.Information,"测试");
            //throw new Exception("测试错误");
            var nameInt = Convert.ToInt32(name);
            return name;
        }
    }
}
