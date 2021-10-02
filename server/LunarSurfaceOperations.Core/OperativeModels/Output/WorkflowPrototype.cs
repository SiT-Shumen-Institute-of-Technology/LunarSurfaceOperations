namespace LunarSurfaceOperations.Core.OperativeModels.Output
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public class WorkflowPrototype : IWorkflowPrototype
    {
        public WorkflowPrototype(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}