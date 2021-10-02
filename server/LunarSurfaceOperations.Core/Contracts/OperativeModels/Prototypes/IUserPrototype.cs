namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes
{
    public interface IUserPrototype
    {
        string Username { get; }
        string Email { get; }
        string Password { get; }
    }
}