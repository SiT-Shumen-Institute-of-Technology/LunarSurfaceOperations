namespace LunarSurfaceOperations.Core.Processors.MessageAttribute
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public class MessageStringAttributeIntermediaryProcessor : BaseMessageAttributeIntermediaryProcessor<MessageStringAttribute>
    {
        public MessageStringAttributeIntermediaryProcessor([NotNull] MessageStringAttribute attribute) : base(attribute)
        {
        }

        public override Task<IOperationResult<IMessageAttributeLayout>> ConstructLayoutAsync(CancellationToken cancellationToken) => Task.FromResult(this.ConstructLayoutInternally());

        private IOperationResult<IMessageAttributeLayout> ConstructLayoutInternally()
        {
            var operationResult = new OperationResult<IMessageAttributeLayout>();

            operationResult.Data = new MessageStringAttributeLayout(this.Attribute.AttributeName, this.Attribute.Value);
            return operationResult;
        }
    }
}