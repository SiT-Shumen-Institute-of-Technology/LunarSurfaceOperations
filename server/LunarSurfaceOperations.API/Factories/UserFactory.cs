namespace LunarSurfaceOperations.API.Factories
{
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.ViewModels.User;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public class UserFactory : IUserFactory
    {
        public UserViewModel ToViewModel(IUserLayout userLayout)
        {
            if (userLayout is null)
                return null;

            return new UserViewModel { Id = userLayout.Id, Username = userLayout.Username };
        }
    }
}