namespace LunarSurfaceOperations.API.ViewModels.Message
{
    using MongoDB.Bson;

    public class ApproveMessageRequest
    {
        public ObjectId MessageId { get; set; }
    }
}