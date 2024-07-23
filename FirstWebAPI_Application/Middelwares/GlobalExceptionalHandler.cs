using System.Net;

namespace FirstWebAPI_Application.Middelwares
{
    public class GlobalExceptionalHandler
    {
        private readonly ILogger<GlobalExceptionalHandler> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionalHandler(ILogger<GlobalExceptionalHandler> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();

                //logging error
                _logger.LogWarning(errorId, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorObj = new
                {
                    id = errorId,
                    message = "something went wrong !!"

                };

                await context.Response.WriteAsJsonAsync(errorObj);
            }

        }
    }
}
