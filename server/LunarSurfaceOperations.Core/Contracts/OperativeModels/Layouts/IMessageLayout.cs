namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    using MongoDB.Bson;

    public interface IMessageLayout : ILayout
    {
        ObjectId WorkspaceId { get; }
        string Text { get; }
    }
}