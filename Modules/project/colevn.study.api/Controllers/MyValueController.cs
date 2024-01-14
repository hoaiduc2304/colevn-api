using colevn.study.business.Filter;
using DNH.Infrastructure.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace colevn.study.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[EnableCors(MainConfig.CorsPublic)]
    //[Authorize]
    public class MyValueController : ControllerBase
    {
        private readonly ILogger<StartsWithColevnResourceFilter> _logger;
        public MyValueController() { }
        [HttpGet("send-message")]
        [StartsWithColevnResourceFilter()]
        public async Task<IActionResult> sendMessage(string message)
        {
            PageResultV1 resultData = new PageResultV1();
            
            resultData = new PageResultV1("Chào mới tới ColeVN - Nội dung của bạn là:"+message);
            return Ok(resultData);
        }
    }
}