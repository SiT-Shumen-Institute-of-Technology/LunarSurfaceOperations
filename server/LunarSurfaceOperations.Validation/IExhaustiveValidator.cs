namespace LunarSurfaceOperations.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using Quantum.DMS.Utilities;

    public class ExhaustiveFluentValidator<TEntity> : IExhaustiveValidator<TEntity>
    {
        private readonly IReadOnlyCollection<IValidator<TEntity>> _fluentValidators;

        public ExhaustiveFluentValidator(IEnumerable<IValidator<TEntity>> fluentValidators)
        {
            this._fluentValidators = fluentValidators.OrEmptyIfNull().IgnoreNullValues().ToList().AsReadOnly();
        }

        public async Task<IOperationResult> ValidateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            foreach (var fluentValidator in this._fluentValidators)
            {
                var validationResult = await fluentValidator.ValidateAsync(entity, cancellationToken);
                if (validationResult.IsValid)
                    continue;
                
                foreach (var error in validationResult.Errors.OrEmptyIfNull().IgnoreNullValues())
                    operationResult.AddErrorMessage(error.ErrorMessage);
            }

            return operationResult;
        }
    }
}