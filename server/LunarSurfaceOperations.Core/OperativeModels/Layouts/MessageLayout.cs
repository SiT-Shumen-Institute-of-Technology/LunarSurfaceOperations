namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using System;
    using System.Collections.Generic;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Data.Enums;
    using MongoDB.Bson;

    public record MessageLayout : BaseLayout, IMessageLayout
    {
        private readonly List<IMessageAttributeLayout> _attributes = new();

        public MessageLayout(ObjectId id, ObjectId workspaceId, string text, IUserLayout author, DateTime timestamp, MessageStatus status)
            : base(id)
        {
            this.WorkspaceId = workspaceId;
            this.Text = text;
            this.Author = author;
            this.Timestamp = timestamp;
            this.Status = status;
        }

        public ObjectId WorkspaceId { get; }
        public string Text { get; }
        public IReadOnlyCollection<IMessageAttributeLayout> Attributes => this._attributes.AsReadOnly();
        public IUserLayout Author { get; }
        public DateTime Timestamp { get; }
        public MessageStatus Status { get; }

        public void AddAttribute(IMessageAttributeLayout attribute)
        {
            if (attribute is not null)
                this._attributes.Add(attribute);
        }
    }
}