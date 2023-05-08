using GlobalProject.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalProject.Api.Controllers
{
    /// <summary>
    /// redis缓存    
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        RedisServer redisServer = new RedisServer();
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [HttpPost]
        public void SetKey([FromBody]string value)
        {

            redisServer.Set("name", value);
        }
    }
}
