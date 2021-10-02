namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;

    public class UserService : BaseService<IUserRepository, User, IUserPrototype, IUserLayout>, IUserService
    {
        private readonly IPasswordHashingService _hashingService;
        
        public UserService([NotNull] IUserRepository repository, [NotNull] IExhaustiveValidator<IUserPrototype> validator, [NotNull] IPasswordHashingService hashingService)
            : base(repository, validator)
        {
            this._hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        protected override IOperationResult EnhanceDatabaseModel(User databaseModel, IUserPrototype prototype)
        {
            var operationResult = new OperationResult();
            operationResult.ValidateNotNull(databaseModel);
            operationResult.ValidateNotNull(prototype);
            if (operationResult.Success is false)
                return operationResult;

            var hashPassword = this._hashingService.Hash(prototype.Password);
            if (hashPassword.Success == false)
                return operationResult.AppendErrorMessages(hashPassword);

            databaseModel.Username = prototype.Username;
            databaseModel.Email = prototype.Email;
            databaseModel.Password = hashPassword.Data.HashedValue;
            databaseModel.Salt = hashPassword.Data.Salt;

            return operationResult;
        }

        protected override IOperationResult<IUserLayout> ConstructLayout(User entity)
        {
            var operationResult = new OperationResult<IUserLayout>();
            
            operationResult.ValidateNotNull(entity);
            if (operationResult.Success == false)
                return operationResult;

            operationResult.Data = new UserLayout(entity.Username, entity.Email);
                return operationResult;
        }
    }
}