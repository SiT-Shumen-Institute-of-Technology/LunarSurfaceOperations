namespace LunarSurfaceOperations.Core.Authentication
{
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public class AuthenticationContext : IAuthenticationContext
    {
        private IUserLayout _currentUser;
        
        public bool IsAuthenticated { get; private set; }
        public IUserLayout CurrentUser { get; }

        public void Authenticate(IUserLayout userLayout)
        {
            if (userLayout is null)
                return;

            this.IsAuthenticated = true;
            this._currentUser = userLayout;
        }
    }
}