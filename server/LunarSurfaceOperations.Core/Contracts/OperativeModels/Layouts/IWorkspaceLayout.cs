namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    public interface IWorkspaceLayout : ILayout
    {
        string Name { get; }
        string Description { get; }
    }
}