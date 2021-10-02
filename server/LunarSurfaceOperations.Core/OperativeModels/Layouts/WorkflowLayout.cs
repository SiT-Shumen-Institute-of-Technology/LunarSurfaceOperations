namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record WorkflowLayout : BaseLayout, IWorkflowLayout
    {
        public WorkflowLayout(ObjectId id, string name, string description)
            : base(id)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}