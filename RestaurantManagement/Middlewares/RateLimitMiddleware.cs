using System.Collections.Concurrent;

namespace RestaurantManagement.Middlewares;

public class RateLimitMiddleware(RequestDelegate next, int requestLimit, TimeSpan timeSpan, IHttpContextAccessor contextAccessor)
{

    private readonly RequestDelegate _next = next;
    private readonly int _requestLimit = requestLimit;
    private readonly TimeSpan _timeSpan = timeSpan;
    private readonly ConcurrentDictionary<string, List<DateTime>> _requestTimes = new();
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); //ip addresin aliriq
        var now = DateTime.UtcNow;
        var requestLog = _requestTimes.GetOrAdd(clientId, new List<DateTime>());
        lock (requestLog)
        {
            requestLog.RemoveAll(timeStamp => timeStamp <= now - _timeSpan);
            if (requestLog.Count >= _requestLimit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers.RetryAfter = _timeSpan.TotalMinutes.ToString();
                return;
            }
            requestLog.Add(now);
        }
        await _next(context);

    }
}
