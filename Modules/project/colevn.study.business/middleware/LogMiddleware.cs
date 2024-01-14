using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace colevn.study.business.middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly string ExpectedHeaderValue = "ducdepdzai";

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originBody = context.Response.Body;
            try
            {
                // Kiểm tra nếu đường dẫn chứa "swagger"
                if (context.Request.Path.StartsWithSegments("/swagger"))
                {
                    // Nếu là trang Swagger, bỏ qua middleware và tiếp tục xử lý request
                    await _next(context);
                    return;
                }
                // Lấy giá trị của header Authorization
                var authorizationHeaderValue = context.Request.Headers["Authorization"];

                // Kiểm tra giá trị của header Authorization
                if (authorizationHeaderValue != ExpectedHeaderValue)
                {
                    // Log và trả về lỗi 401 Unauthorized nếu giá trị không khớp
                    
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                // Tiếp tục chuyển request đến middleware hoặc endpoint tiếp theo trong pipeline
                await _next(context);
            }
            finally
            {
                context.Response.Body = originBody;
            }
        }
    }
}
