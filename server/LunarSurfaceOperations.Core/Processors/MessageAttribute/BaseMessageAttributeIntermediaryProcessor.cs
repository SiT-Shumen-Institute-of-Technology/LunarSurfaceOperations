namespace LunarSurfaceOperations.Core.Processors.MessageAttribute
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public abstract class BaseMessageAttributeIntermediaryProcessor<T> : IMessageAttributeIntermediaryProcessor
        where T : class, IMessageAttribute
    {
        [NotNull]
        protected T Attribute { get; }

        protected BaseMessageAttributeIntermediaryProcessor([NotNull] T attribute)
        {
            this.Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
        }

        public abstract Task<IOperationResult<IMessageAttributeLayout>> ConstructLayoutAsync(CancellationToken cancellationToken);
    }
}