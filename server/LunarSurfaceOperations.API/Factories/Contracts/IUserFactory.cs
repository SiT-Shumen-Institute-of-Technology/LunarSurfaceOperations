namespace LunarSurfaceOperations.API.Factories.Contracts
{
    using LunarSurfaceOperations.API.ViewModels.User;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public interface IUserFactory
    {
        UserViewModel ToViewModel(IUserLayout userLayout);
    }
}