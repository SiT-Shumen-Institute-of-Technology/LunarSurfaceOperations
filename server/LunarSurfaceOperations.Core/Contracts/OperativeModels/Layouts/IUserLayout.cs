namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    public interface IUserLayout : ILayout
    {
        string Username { get; }
        string Email { get; }
    }
}