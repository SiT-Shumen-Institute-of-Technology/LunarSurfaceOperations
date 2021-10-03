namespace LunarSurfaceOperations.API.Factories.Contracts.MessageAttributes
{
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Utilities.TypedProcessors;

    public interface IMessageAttributeLayoutProcessor : ITypedProcessor<IMessageAttributeLayout, IMessageAttributeViewModel>
    {
    }
}