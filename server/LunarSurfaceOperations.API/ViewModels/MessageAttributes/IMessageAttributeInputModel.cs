namespace LunarSurfaceOperations.API.ViewModels.MessageAttributes
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public interface IMessageAttributeInputModel
    {
        IMessageAttributePrototype ConstructPrototype();
    }
}