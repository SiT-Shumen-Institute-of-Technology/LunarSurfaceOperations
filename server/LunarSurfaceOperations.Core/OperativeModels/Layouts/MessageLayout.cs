namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record MessageLayout : BaseLayout, IMessageLayout
    {
        public MessageLayout(ObjectId id, ObjectId workspaceId, string text)
            : base(id)
        {
            this.WorkspaceId = workspaceId;
            this.Text = text;
        }

        public ObjectId WorkspaceId { get; }
        public string Text { get; }
    }
}