namespace LunarSurfaceOperations.API.ViewModels.Message
{
    using MongoDB.Bson;

    public class MessageViewModel
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
    }
}