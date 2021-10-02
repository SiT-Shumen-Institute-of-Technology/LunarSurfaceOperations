namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    public interface IWorkflowLayout : ILayout
    {
        string Name { get; }
        string Description { get; }
    }
}