namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    using MongoDB.Bson;

    public interface ILayout
    {
        ObjectId Id { get; }
    }
}