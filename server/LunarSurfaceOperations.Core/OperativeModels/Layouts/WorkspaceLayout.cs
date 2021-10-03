namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record WorkspaceLayout : BaseLayout, IWorkspaceLayout
    {
        public WorkspaceLayout(ObjectId id, string name, string description, IUserLayout owner)
            : base(id)
        {
            this.Name = name;
            this.Description = description;
            this.Owner = owner;
        }

        public string Name { get; }
        public string Description { get; }
        public IUserLayout Owner { get; }
    }
}