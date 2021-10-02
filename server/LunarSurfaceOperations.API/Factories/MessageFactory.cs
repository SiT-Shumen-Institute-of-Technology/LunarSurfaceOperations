namespace LunarSurfaceOperations.API.Factories
{
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.ViewModels.Message;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public class MessageFactory : IMessageFactory
    {
        public MessageViewModel ToViewModel(IMessageLayout messageLayout)
        {
            if (messageLayout is null)
                return null;

            return new MessageViewModel { Id = messageLayout.Id, Text = messageLayout.Text };
        }
    }
}