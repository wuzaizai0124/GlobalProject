using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkywalkingController : ControllerBase
    {
        private readonly IEntrySegmentContextAccessor segContext;
        public SkywalkingController(IEntrySegmentContextAccessor skyContext)
        {
            segContext = skyContext;
        }
        [HttpGet]
        [Route("gettraceid")]
        public string GetTraceId()
        {
            //获取全局的skywalking的TracId
            var TraceId = segContext.Context.TraceId;
            segContext.Context.Span.AddLog(LogEvent.Message($"SkywalkingTest1---Worker running at: {DateTime.Now}"));
            //segContext.Context.Span.AddLog(LogEvent.Message($"SkywalkingTest1---Worker running at--end: {DateTime.Now}"));
            return TraceId;
        }
    }
}
