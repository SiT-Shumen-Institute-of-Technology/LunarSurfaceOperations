namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    using System;
    using System.Collections.Generic;
    using LunarSurfaceOperations.Data.Enums;
    using MongoDB.Bson;

    public interface IMessageLayout : ILayout
    {
        ObjectId WorkspaceId { get; }
        string Text { get; }
        IReadOnlyCollection<IMessageAttributeLayout> Attributes { get; }
        IUserLayout Author { get; }
        DateTime Timestamp { get; }
        MessageStatus Status { get; }
    }
}