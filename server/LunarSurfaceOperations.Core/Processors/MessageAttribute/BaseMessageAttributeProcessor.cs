namespace LunarSurfaceOperations.Core.Processors.MessageAttribute
{
    using LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.TypedProcessors;

    public abstract class BaseMessageAttributeProcessor<T> : BaseTypedProcessor<T, IMessageAttribute, IMessageAttributeIntermediaryProcessor>, IMessageAttributeProcessor
        where T : class, IMessageAttribute
    {
    }
}