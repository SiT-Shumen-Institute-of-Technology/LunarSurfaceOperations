namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using System.Collections.Generic;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record MessageLayout : BaseLayout, IMessageLayout
    {
        private readonly List<IMessageAttributeLayout> _attributes = new();

        public MessageLayout(ObjectId id, ObjectId workspaceId, string text)
            : base(id)
        {
            this.WorkspaceId = workspaceId;
            this.Text = text;
        }

        public ObjectId WorkspaceId { get; }
        public string Text { get; }
        public IReadOnlyCollection<IMessageAttributeLayout> Attributes => this._attributes.AsReadOnly();

        public void AddAttribute(IMessageAttributeLayout attribute)
        {
            if (attribute is not null)
                this._attributes.Add(attribute);
        }
    }
}