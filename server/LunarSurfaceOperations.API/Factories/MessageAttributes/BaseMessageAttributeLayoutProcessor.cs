namespace LunarSurfaceOperations.API.Factories.MessageAttributes
{
    using LunarSurfaceOperations.API.Factories.Contracts.MessageAttributes;
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Utilities.TypedProcessors;

    public abstract class BaseMessageAttributeLayoutProcessor<T> : BaseTypedProcessor<T, IMessageAttributeLayout, IMessageAttributeViewModel>, IMessageAttributeLayoutProcessor
        where T : class, IMessageAttributeLayout
    {
    }
}