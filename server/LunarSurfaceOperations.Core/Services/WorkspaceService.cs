namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
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
    using Quantum.DMS.Utilities;

    public class WorkspaceService : BaseService<IWorkspaceRepository, Workspace, IWorkspacePrototype, IWorkspaceLayout>, IWorkspaceService
    {
        private readonly IAuthenticationContext _authenticationContext;

        public WorkspaceService([NotNull] IWorkspaceRepository repository, [NotNull] IExhaustiveValidator<IWorkspacePrototype> validator, [NotNull] IAuthenticationContext authenticationContext)
            : base(repository, validator)
        {
            this._authenticationContext = authenticationContext ?? throw new ArgumentNullException(nameof(authenticationContext));
        }

        public async Task<IOperationResult< IEnumerable<IWorkspaceLayout>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IWorkspaceLayout>>();

            var getWorkspaces = await this.Repository.GetForUserAsync(this._authenticationContext.CurrentUser.Id, cancellationToken);
            if (getWorkspaces.Success is false)
                return operationResult.AppendErrorMessages(getWorkspaces);

            var layouts = new List<IWorkspaceLayout>();
            foreach (var workspace in getWorkspaces.Data.OrEmptyIfNull().IgnoreNullValues())
            {
                var constructLayout = this.ConstructLayout(workspace);
                if (constructLayout.Success is false)
                    return operationResult.AppendErrorMessages(constructLayout);

                var layout = constructLayout.Data;
                if (layout is not null)
                    layouts.Add(layout);
            }

            operationResult.Data = layouts;
            return operationResult;
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