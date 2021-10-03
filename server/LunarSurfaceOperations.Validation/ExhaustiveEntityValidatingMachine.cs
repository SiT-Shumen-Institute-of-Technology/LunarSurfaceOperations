namespace LunarSurfaceOperations.Validation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using Quantum.DMS.Utilities;

    public class ExhaustiveEntityValidatingMachine<TEntity> : IExhaustiveEntityValidatingMachine
    {
        [NotNull]
        private readonly IValidator<TEntity> _validator;

        [NotNull]
        private readonly TEntity _instanceToValidate;

        public ExhaustiveEntityValidatingMachine([NotNull] IValidator<TEntity> validator, TEntity instanceToValidate)
        {
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this._instanceToValidate = instanceToValidate;
        }

        public async Task<IOperationResult> TriggerValidationAsync(CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            var validationResult = await this._validator.ValidateAsync(this._instanceToValidate, cancellationToken);
            if (validationResult.IsValid)
                return operationResult;

            foreach (var error in validationResult.Errors.OrEmptyIfNull().IgnoreNullValues())
                operationResult.AddErrorMessage(error.ErrorMessage);
            return operationResult;
        }
    }
}