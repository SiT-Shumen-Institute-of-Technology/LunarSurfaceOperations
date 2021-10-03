namespace LunarSurfaceOperations.Core.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Validators;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Validation.Contracts;

    public class ExhaustiveValidatorBridge<TEntity, TProperty> : AsyncPropertyValidator<TEntity, TProperty>
    {
        [NotNull]
        private readonly IExhaustiveValidator<TProperty> _validator;

        public ExhaustiveValidatorBridge([NotNull] IExhaustiveValidator<TProperty> validator)
        {
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public override string Name => "Exhaustive validator bridge";

        public override async Task<bool> IsValidAsync(ValidationContext<TEntity> context, TProperty value, CancellationToken cancellation)
        {
            var validationResult = await this._validator.ValidateAsync(value, cancellation);

            foreach (var error in validationResult.Errors)
                context.AddFailure(error);

            return validationResult.Success;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => string.Empty;
    }
}