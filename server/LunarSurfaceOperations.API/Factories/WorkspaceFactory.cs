namespace LunarSurfaceOperations.API.Factories
{
    using System;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.ViewModels.Workspace;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public class WorkspaceFactory : IWorkspaceFactory
    {
        [NotNull]
        private readonly IUserFactory _userFactory;

        public WorkspaceFactory([NotNull] IUserFactory userFactory)
        {
            this._userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
        }

        public WorkspaceViewModel ToViewModel(IWorkspaceLayout workspaceLayout)
        {
            if (workspaceLayout is null)
                return null;

            var owner = this._userFactory.ToViewModel(workspaceLayout.Owner);
            return new WorkspaceViewModel
            {
                Id = workspaceLayout.Id,
                Name = workspaceLayout.Name,
                Description = workspaceLayout.Description,
                Owner = owner
            };
        }
    }
}