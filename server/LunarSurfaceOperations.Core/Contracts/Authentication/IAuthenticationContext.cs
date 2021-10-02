namespace LunarSurfaceOperations.Core.Contracts.Authentication
{
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public interface IAuthenticationContext
    {
        bool IsAuthenticated { get; }
        
        [NotNull]
        IUserLayout CurrentUser { get; }

        void Authenticate(IUserLayout userLayout);
    }
}