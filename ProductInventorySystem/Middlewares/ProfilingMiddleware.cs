using System.Diagnostics;

namespace Inventory.Api.Middlewares
{
    public class ProfilingMiddleware
    {

        private readonly ILogger<ProfilingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ProfilingMiddleware(ILogger<ProfilingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();

            var duration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation($"Request `{context.Request.Path}` took `{duration}ms` to execute");
        }
    }
}
