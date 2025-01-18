namespace EquipmentHostingService.API.Middlewares;

public class ApiKeyMiddleware
{
    readonly RequestDelegate _next;
    readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey) ||
            extractedApiKey != _configuration["ApiKey"])
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized. Missing or invalid API Key.");
            return;
        }

        await _next(context);
    }
}
