namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public interface IMessageLayout : ILayout
    {
        ObjectId WorkspaceId { get; }
        string Text { get; }
        IReadOnlyCollection<IMessageAttributeLayout> Attributes { get; }
    }
}