using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Web.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                TraceId = context.TraceIdentifier
            };

            switch (exception)
            {
                case NotFoundException:
                    problemDetails.Type = HttpProblemTypes.NotFound;
                    problemDetails.Title = "Resource Not Found";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = exception.Message;
                    break;
                case ValidationException:
                    problemDetails.Type = HttpProblemTypes.ValidationError;
                    problemDetails.Title = "Validation Error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = exception.Message;
                    break;
                case DuplicateSkuException:
                    problemDetails.Type = HttpProblemTypes.Conflict;
                    problemDetails.Title = "Resource Conflict";
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Detail = exception.Message;
                    break;
                case InsufficientStockException:
                case InvalidPaymentException:
                    problemDetails.Type = HttpProblemTypes.ValidationError;
                    problemDetails.Title = "Business Rule Violation";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = exception.Message;
                    break;
                case BusinessException:
                    problemDetails.Type = HttpProblemTypes.InternalServerError;
                    problemDetails.Title = "Server Error";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = exception.Message;
                    break;
                default:
                    problemDetails.Type = HttpProblemTypes.InternalServerError;
                    problemDetails.Title = "Server Error";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = "An unexpected error occurred";
                    break;
            }

            // Add additional context for development
            if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                problemDetails.Extensions["exception"] = exception.ToString();
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonResponse = JsonSerializer.Serialize(problemDetails, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

// Replaced by ProblemDetails class for better HTTP compliance
}