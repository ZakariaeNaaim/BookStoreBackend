using Application.Dtos.Common;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred while executing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ApiResponseDto<string> { Success = false };

            switch (exception)
            {
                case NotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ex.Message;
                    break;

                case ValidationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ex.Message;
                    // Ideally pass validation errors too, but Message is string in base DTO.
                    // For now, simple message. Enhanced DTO could handle list.
                    if (ex.Errors != null && ex.Errors.Any())
                    {
                        response.Message = $"{ex.Message} Errors: {string.Join("; ", ex.Errors)}";
                    }
                    break;

                case UnauthorizedException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ex.Message;
                    break;
                
                case ApiException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Internal Server Error. Please contact support.";
                    break;
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
