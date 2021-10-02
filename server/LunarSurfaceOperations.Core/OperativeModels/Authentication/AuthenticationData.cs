namespace LunarSurfaceOperations.Core.OperativeModels.Authentication
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Authentication;
    using MongoDB.Bson;

    public record AuthenticationData : IAuthenticationData
    {
        public AuthenticationData(ObjectId id)
        {
            this.Id = id;
        }

        public ObjectId Id { get; }
    }
}