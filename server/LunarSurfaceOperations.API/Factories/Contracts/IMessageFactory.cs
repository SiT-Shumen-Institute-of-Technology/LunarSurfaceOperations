namespace LunarSurfaceOperations.API.Factories.Contracts
{
    using LunarSurfaceOperations.API.ViewModels.Message;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public interface IMessageFactory
    {
        MessageViewModel ToViewModel(IMessageLayout messageLayout);
    }
}