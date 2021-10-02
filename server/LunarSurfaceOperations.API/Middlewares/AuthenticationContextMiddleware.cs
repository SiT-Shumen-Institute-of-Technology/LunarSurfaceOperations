namespace LunarSurfaceOperations.API.Middlewares
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;

    public class AuthenticationContextMiddleware
    {
        [NotNull]
        private readonly RequestDelegate _next;

        public AuthenticationContextMiddleware([NotNull] RequestDelegate next)
        {
            this._next = next ?? throw new ArgumentNullException(nameof(next));
        }

        [UsedImplicitly]
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var success = await ProcessAuthenticationAsync(httpContext);
            if (success)
                await this._next(httpContext);
        }

        private static async Task<bool> ProcessAuthenticationAsync([NotNull] HttpContext httpContext)
        {
            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));

            var user = httpContext.User;
            var identity = user.Identity;
            if (identity is null || identity.IsAuthenticated == false)
                return true;

            var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (string.IsNullOrWhiteSpace(idClaim))
                return true;

            // retrieve the application user within the tenant.
            var userService = httpContext.RequestServices.GetRequiredService<IUserService>();

            if (ObjectId.TryParse(idClaim, out var id) == false)
                return true;
            
            var getUserResult = await userService.GetAsync(id, httpContext.RequestAborted).ConfigureAwait(false);
            if (getUserResult.Success == false)
            {
                await httpContext.Response.ReturnUnauthorizedAsync(getUserResult.ToString());
                return false;
            }

            var authenticatedUser = getUserResult.Data;
            if (authenticatedUser is null)
            {
                await httpContext.Response.ReturnUnauthorizedAsync("Cannot retrieve the application identity.");
                return false;
            }

            // Set the authentication context and assign it to the service.
            var authenticationContext = httpContext.RequestServices.GetRequiredService<IAuthenticationContext>();
            authenticationContext.Authenticate(authenticatedUser);

            return true;
        }
    }
}