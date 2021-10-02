namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Authentication;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Services.ScopeIdentification;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;

    public class UserService : BaseService<IUserRepository, User, EmptyScopeIdentification<User>, IUserPrototype, IUserLayout>, IUserService
    {
        private readonly IPasswordHashingService _hashingService;

        public UserService([NotNull] IUserRepository repository, [NotNull] IExhaustiveValidator<IUserPrototype> validator, [NotNull] IPasswordHashingService hashingService)
            : base(repository, validator)
        {
            this._hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        public async Task<IOperationResult<IAuthenticationData>> GetAuthenticationDataAsync(string username, string password, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IAuthenticationData>();

            operationResult.ValidateNotNullOrWhitespace(username);
            operationResult.ValidateNotNullOrWhitespace(password);
            if (operationResult.Success is false)
                return operationResult;

            var getUser = await this.Repository.GetByUsernameAsync(username, cancellationToken);
            if (getUser.Success == false)
                return operationResult.AppendErrorMessages(getUser);

            var user = getUser.Data;
            operationResult.ValidateNotNull(user, AuthenticationMessages.MissingUser);
            if (operationResult.Success == false)
                return operationResult;

            var validatePassword = this._hashingService.Validate(password, user.Password, user.Salt);
            if (validatePassword is false)
                operationResult.AddErrorMessage(AuthenticationMessages.InvalidPassword);
            else
                operationResult.Data = new AuthenticationData(user.Id);

            return operationResult;
        }

        public async Task<OperationResult<IUserLayout>> GetAsync(ObjectId userId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IUserLayout>();

            var getUser = await this.GetEntityByIdAsync(userId, cancellationToken);
            if (getUser.Success is false)
                return operationResult.AppendErrorMessages(getUser);

            var user = getUser.Data;
            if (user is null)
                return operationResult;

            var constructLayout = this.ConstructLayout(getUser.Data);
            if (constructLayout.Success is false)
                operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        public async Task<OperationResult<IEnumerable<IUserLayout>>> GetManyByUsernameAsync(IEnumerable<string> usernames, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IUserLayout>>();

            var getUsers = await this.Repository.GetManyByUsernameAsync(usernames, cancellationToken);
            if (getUsers.Success is false)
                return operationResult.AppendErrorMessages(getUsers);

            var constructManyLayouts = this.ConstructManyLayouts(getUsers.Data);
            if (constructManyLayouts.Success is false)
                return operationResult.AppendErrorMessages(constructManyLayouts);

            operationResult.Data = constructManyLayouts.Data;
            return operationResult;
        }

        public async Task<OperationResult<IEnumerable<IUserLayout>>> GetManyAsync(IEnumerable<ObjectId> identifiers, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IUserLayout>>();

            var getUsers = await this.Repository.GetManyAsync(identifiers, cancellationToken);
            if (getUsers.Success is false)
                return operationResult.AppendErrorMessages(getUsers);

            var constructManyLayouts = this.ConstructManyLayouts(getUsers.Data);
            if (constructManyLayouts.Success is false)
                return operationResult.AppendErrorMessages(constructManyLayouts);

            operationResult.Data = constructManyLayouts.Data;
            return operationResult;
        }

        public Task<IOperationResult<IUserLayout>> CreateAsync(IUserPrototype prototype, CancellationToken cancellationToken)
            => this.CreateInternallyAsync(new EmptyScopeIdentification<User>(), prototype, cancellationToken);

        public Task<IOperationResult<IUserLayout>> UpdateAsync(ObjectId id, IUserPrototype prototype, CancellationToken cancellationToken) 
            => this.UpdateInternallyAsync(id, new EmptyScopeIdentification<User>(), prototype, cancellationToken);

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

            operationResult.Data = new UserLayout(entity.Id, entity.Username, entity.Email);
            return operationResult;
        }

        protected override Task<IOperationResult<User>> GetEntityByIdAsync(ObjectId entityId, EmptyScopeIdentification<User> identification, CancellationToken cancellationToken)
            => this.GetEntityByIdAsync(entityId, cancellationToken);

        private Task<IOperationResult<User>> GetEntityByIdAsync(ObjectId entityId, CancellationToken cancellationToken)
            => this.Repository.GetAsync(entityId, cancellationToken);
    }
}