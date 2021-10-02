namespace LunarSurfaceOperations.Data.Contracts
{
    using MongoDB.Bson;

    public interface IEntity
    {
        ObjectId Id { get; set; }
    }
}