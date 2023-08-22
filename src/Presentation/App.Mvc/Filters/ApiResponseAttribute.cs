using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Mvc.Filters
{
    public class ApiResponseAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var response = new ApiResponse();

            var result = context.Result;

            if (result is OkObjectResult)
            {
                var ok = result as OkObjectResult;
                response.SetData(ok.Value);
                response.SetStatusCode(HttpStatusCode.OK);
            }
            else if (result is OkResult)
            {
                response.SetStatusCode(HttpStatusCode.NoContent);
            }
            else if (result is EmptyResult)
            {
                response.SetStatusCode(HttpStatusCode.NoContent);
            }
            else if (result is BadRequestObjectResult)
            {
                var value = (result as ObjectResult).Value;

                if (value is string)
                    response.SetError(value as string);
                else
                    response.SetData(value);
                
                response.SetStatusCode(HttpStatusCode.BadRequest);                
            }
            else if (result is IActionResult)
            {
                //When the controller not return a object with implementation IActionResult                        
                response.SetStatusCode(HttpStatusCode.Accepted);
            }

            context.SetResponse(response);

            base.OnResultExecuting(context);
        }
    }

    internal static class ResultExecutedContextExtension
    {
        public static void SetResponse(this ResultExecutingContext context, ApiResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    context.Result = new NoContentResult();
                    break;
                default:
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = (int)response.StatusCode
                    };
                    break;
            }
        }
    }
}
