namespace LunarSurfaceOperations.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Quantum.DMS.Utilities;

    public class ExhaustiveValidator<TEntity> : IExhaustiveValidator<TEntity>
    {
        [NotNull]
        private readonly IServiceProvider _serviceProvider;

        public ExhaustiveValidator([NotNull] IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<IOperationResult> ValidateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            var validatingMachines = this.ComposeAllValidatingMachines(entity);
            foreach (var validator in validatingMachines.OrEmptyIfNull().IgnoreNullValues())
            {
                var validationResult = await validator.TriggerValidationAsync(cancellationToken).ConfigureAwait(false);
                if (validationResult.Success == false)
                    return operationResult.AppendErrorMessages(validationResult);
            }

            return operationResult;
        }

        private IEnumerable<IExhaustiveEntityValidatingMachine> ComposeAllValidatingMachines(TEntity entity)
        {
            if (entity is null)
                return Enumerable.Empty<IExhaustiveEntityValidatingMachine>();

            var allValidatingMachines = new List<IExhaustiveEntityValidatingMachine>();

            var entityType = entity.GetType();
            AddValidators(entityType);

            var baseType = entityType.BaseType;
            while (baseType != null)
            {
                AddValidators(baseType);
                baseType = baseType.BaseType;
            }

            var interfaces = entityType.GetInterfaces();
            foreach (var implementedInterfaceType in interfaces)
                AddValidators(implementedInterfaceType);

            return allValidatingMachines;

            void AddValidators(Type validatedType)
            {
                var principalType = typeof(TEntity);
                if (principalType.IsAssignableFrom(validatedType) == false)
                    return;

                var validatorType = typeof(IValidator<>).MakeGenericType(validatedType);
                var registeredValidators = this._serviceProvider.GetServices(validatorType);
                foreach (var registeredValidator in registeredValidators)
                {
                    var validatingMachineType = typeof(ExhaustiveEntityValidatingMachine<>).MakeGenericType(validatedType);
                    if (Activator.CreateInstance(validatingMachineType, registeredValidator, entity) is IExhaustiveEntityValidatingMachine validatingMachine)
                        allValidatingMachines.Add(validatingMachine);
                }
            }
        }
    }
}