using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.Shared;


namespace ECommWeb.Infrastructure.src.Middleware
{
    public class ExceptionHanlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid Data");
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized access");
            }
            // catch (DbUpdateException ex)
            // {
            //     context.Response.StatusCode = 400;
            //     await context.Response.WriteAsync(ex.Message);
            // }
            catch (AppException ex)
            {
                context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsync(ex.Message);
            }
            // catch (Exception ex)
            // {
            //     context.Response.StatusCode = 500;
            //     await context.Response.WriteAsync(ex.Message);
            // }

        }
    }
}
