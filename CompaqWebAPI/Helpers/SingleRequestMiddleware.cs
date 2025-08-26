using System.Globalization;

namespace CompaqWebAPI.Helpers
{
    public class SingleRequestMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public SingleRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _next(context);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public static class SingleRequestMiddlewareExtension
    {
        public static IApplicationBuilder UseSingleRequestMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SingleRequestMiddleware>();
        }
    }

}
