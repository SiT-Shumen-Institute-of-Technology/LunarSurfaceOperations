namespace LunarSurfaceOperations.API.Hubs
{
    using System.Threading.Tasks;
    using LunarSurfaceOperations.API.ViewModels.Message;

    public interface IMessageHubClient
    {
        Task ReceiveMessage(MessageViewModel messageViewModel);
        Task UpdateMessage(MessageViewModel messageViewModel);
    }
}