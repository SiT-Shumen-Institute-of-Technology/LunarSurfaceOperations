namespace LunarSurfaceOperations.API.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using MongoDB.Bson;

    public class BaseHub<T> : Hub<T>
        where T : class
    {
        protected static ObjectId ConvertToId( string id)
        {
            if (ObjectId.TryParse(id, out var objectId) == false)
                throw new HubException("The provided identifier is not valid");

            return objectId;
        }
    }
}