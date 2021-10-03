namespace LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute
{
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.TypedProcessors;

    public interface IMessageAttributeProcessor : ITypedProcessor<IMessageAttribute, IMessageAttributeIntermediaryProcessor>
    {
    }
}