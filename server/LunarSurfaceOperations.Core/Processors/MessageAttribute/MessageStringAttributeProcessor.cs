namespace LunarSurfaceOperations.Core.Processors.MessageAttribute
{
    using LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute;
    using LunarSurfaceOperations.Data.Models;

    public class MessageStringAttributeProcessor : BaseMessageAttributeProcessor<MessageStringAttribute>
    {
        protected override IMessageAttributeIntermediaryProcessor Process(MessageStringAttribute entity) => new MessageStringAttributeIntermediaryProcessor(entity);
    }
}