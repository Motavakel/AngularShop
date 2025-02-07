using Domain.Exceptions;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

public class MiddlewareExceptionHandler
{
    private readonly RequestDelegate _next;

    public MiddlewareExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = HandleException(context, exception, options);
            await context.Response.WriteAsync(result);
        }
    }

    private static string HandleException(HttpContext context, Exception exception, JsonSerializerOptions options)
    {
        context.Response.ContentType = "application/json";

        ApiToReturn apiResponse = null!;

        switch (exception)
        {
            case NotFoundEntityException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                apiResponse = new ApiToReturn(404, notFoundException.Messages ?? new List<string> { notFoundException.Message });
                break;
            case BadRequestEntityException badRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                apiResponse = new ApiToReturn(400, badRequestException.Messages ?? new List<string> { badRequestException.Message });
                break;
            case ValidationEntityException validationEntityException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                apiResponse = new ApiToReturn(400, validationEntityException.Messages ?? new List<string> { validationEntityException.Message });
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                apiResponse = new ApiToReturn(500, exception.Message);
                break;
        }

        return JsonSerializer.Serialize(apiResponse, options);
    }
}
