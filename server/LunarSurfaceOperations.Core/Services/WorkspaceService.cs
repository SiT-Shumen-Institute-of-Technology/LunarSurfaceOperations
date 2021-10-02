namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;

    public class WorkspaceService : BaseService<IWorkspaceRepository, Workspace, IWorkspacePrototype, IWorkspaceLayout>, IWorkspaceService
    {
        private readonly IAuthenticationContext _authenticationContext;

        public WorkspaceService([NotNull] IWorkspaceRepository repository, [NotNull] IExhaustiveValidator<IWorkspacePrototype> validator, [NotNull] IAuthenticationContext authenticationContext)
            : base(repository, validator)
        {
            this._authenticationContext = authenticationContext ?? throw new ArgumentNullException(nameof(authenticationContext));
        }

        protected override IOperationResult EnhanceDatabaseModel(Workspace databaseModel, IWorkspacePrototype prototype)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(databaseModel);
            operationResult.ValidateNotNull(prototype);
            if (operationResult.Success == false)
                return operationResult;

            databaseModel.Name = prototype.Name;
            databaseModel.Description = prototype.Description;
            databaseModel.OwnerId = this._authenticationContext.CurrentUser.Id;
            return operationResult;
        }

        protected override IOperationResult<IWorkspaceLayout> ConstructLayout(Workspace entity)
        {
            var operationResult = new OperationResult<IWorkspaceLayout>();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success == false)
                return operationResult;

            operationResult.Data = new WorkspaceLayout(entity.Id, entity.Name, entity.Description);
            return operationResult;
        }
    }
}