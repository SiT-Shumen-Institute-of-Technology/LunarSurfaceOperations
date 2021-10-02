namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Authentication
{
    using MongoDB.Bson;

    public interface IAuthenticationData
    {
        ObjectId Id { get; }
    }
}