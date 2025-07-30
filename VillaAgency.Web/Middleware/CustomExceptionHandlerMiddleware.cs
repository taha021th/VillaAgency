//using FluentValidation;
//using System.Net;
//using System.Text.Json;

//namespace VillaAgency.Web.Middleware;

//public class CustomExceptionHandlerMiddleware
//{
//    private readonly RequestDelegate _next;

//    public CustomExceptionHandlerMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (ValidationException ex)
//        {
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400

//            // ساخت یک پاسخ خطای خوانا
//            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
//            var result = JsonSerializer.Serialize(new { errors });

//            await context.Response.WriteAsync(result);
//        }
//    }

//}
