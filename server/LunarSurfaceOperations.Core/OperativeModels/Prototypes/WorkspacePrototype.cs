namespace LunarSurfaceOperations.Core.OperativeModels.Prototypes
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public record WorkspacePrototype : IWorkspacePrototype
    {
        public WorkspacePrototype(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}