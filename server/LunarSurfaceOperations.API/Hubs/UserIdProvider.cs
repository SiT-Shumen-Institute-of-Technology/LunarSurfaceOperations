namespace LunarSurfaceOperations.API.Hubs
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.SignalR;

    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection) => connection?.User?.FindFirst(ClaimTypes.Sid)?.Value;
    }
}