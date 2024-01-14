using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace colevn.study.business.Filter
{
    public class StartsWithColevnResourceFilter : Attribute, IResourceFilter
    {
        private readonly ILogger<StartsWithColevnResourceFilter> _logger;

        public StartsWithColevnResourceFilter()
        {
            
        }

        public  async void OnResourceExecuting(ResourceExecutingContext context)
        {
            //string content;
            string content = context.HttpContext.Request.Query["message"];
            //using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
            //{
            //    content = await reader.ReadToEndAsync();
            //}

            // Kiểm tra nếu nội dung bắt đầu bằng "Colevn"
            if (!string.IsNullOrEmpty(content) && content.ToString().StartsWith("Colevn", StringComparison.OrdinalIgnoreCase))
            {
                System.Console.WriteLine("OK");
            }
            else
            {
                //_logger.LogInformation("Request rejected due to content not starting with 'Colevn'");
                context.Result = new BadRequestObjectResult("The entry must start with 'Colevn'");
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Có thể thực hiện các công việc sau khi action đã thực thi
        }
    }
}
