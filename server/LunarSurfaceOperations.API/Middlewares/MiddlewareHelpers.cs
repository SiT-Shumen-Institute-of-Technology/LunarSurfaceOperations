namespace LunarSurfaceOperations.API.Middlewares
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;

    public static class MiddlewareHelpers
    {
        public static Task ReturnUnauthorizedAsync([NotNull] this HttpResponse httpResponse, string body = null) => httpResponse.ReturnAsync(401, body);

        public static Task ReturnBadRequestAsync([NotNull] this HttpResponse httpResponse, string body = null) => httpResponse.ReturnAsync(400, body);

        private static async Task ReturnAsync([NotNull] this HttpResponse httpResponse, int statusCode, string body = null)
        {
            if (httpResponse is null)
                throw new ArgumentNullException(nameof(httpResponse));
            
            httpResponse.StatusCode = statusCode;

            if (string.IsNullOrWhiteSpace(body) == false)
                await httpResponse.WriteAsync(body);

            await httpResponse.CompleteAsync();
        }
    }
}