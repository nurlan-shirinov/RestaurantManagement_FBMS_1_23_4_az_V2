using Common.Exceptions;
using Common.GlobalResponse; // Ensure this namespace is correct for your ResponseModel
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace RestaurantManagement.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                // Bypass exception handling for Swagger endpoints
                if (context.Request.Path.StartsWithSegments("/swagger"))
                {
                    throw;
                }

                var messages = new List<string> { error.Message };

                switch (error)
                {
                    case BadRequestException:
                        await WriteError(context, HttpStatusCode.BadRequest, messages);
                        break;
                    case NotFoundException:
                        await WriteError(context, HttpStatusCode.NotFound, messages);
                        break;
                    case ValidationException ex:
                        await WriteValidationErrors(context, HttpStatusCode.BadRequest, ex);
                        break;
                    default:
                        await WriteError(context, HttpStatusCode.InternalServerError, messages);
                        break;
                }
            }
        }

        private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, List<string> messages)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(new ResponseModel(messages));
            await context.Response.WriteAsync(json);
        }

        private static async Task WriteValidationErrors(HttpContext context, HttpStatusCode statusCode, ValidationException ex)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var validationErrors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            var json = JsonSerializer.Serialize(new { errors = validationErrors });
            await context.Response.WriteAsync(json);
        }
    }
}
