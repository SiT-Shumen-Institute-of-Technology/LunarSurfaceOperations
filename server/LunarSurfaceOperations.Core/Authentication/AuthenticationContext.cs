namespace LunarSurfaceOperations.Core.Authentication
{
    using System;
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Resources;

    public class AuthenticationContext : IAuthenticationContext
    {
        private IUserLayout _currentUser;

        public bool IsAuthenticated { get; private set; }

        public IUserLayout CurrentUser
        {
            get => this._currentUser ?? throw new InvalidOperationException(WorkflowMessages.AuthenticationIsRequired);
        }

        public void Authenticate(IUserLayout userLayout)
        {
            if (userLayout is null)
                return;

            this.IsAuthenticated = true;
            this._currentUser = userLayout;
        }
    }
}