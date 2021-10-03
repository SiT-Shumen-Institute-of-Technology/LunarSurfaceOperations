namespace LunarSurfaceOperations.API.Hubs
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;

    public class HubAuthenticationFilter : IHubFilter
    {
        public async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            await TrySetAuthenticatedUser(invocationContext);
            return await next(invocationContext);
        }

        private static async Task TrySetAuthenticatedUser(HubInvocationContext invocationContext)
        {
            var user = invocationContext.Context.User;
            var identity = user?.Identity;
            if (identity is null || identity.IsAuthenticated == false)
                return;

            var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (string.IsNullOrWhiteSpace(idClaim))
                return;

            // retrieve the application user within the tenant.
            var userService = invocationContext.ServiceProvider.GetRequiredService<IUserService>();

            if (ObjectId.TryParse(idClaim, out var id) == false)
                return;

            var getUserResult = await userService.GetAsync(id, invocationContext.Context.ConnectionAborted).ConfigureAwait(false);
            if (getUserResult.Success == false)
                return;

            var authenticatedUser = getUserResult.Data;
            if (authenticatedUser is null)
                return;
            
            var authenticationContext = invocationContext.ServiceProvider.GetRequiredService<IAuthenticationContext>();
            authenticationContext.Authenticate(authenticatedUser);
        }
    }
}