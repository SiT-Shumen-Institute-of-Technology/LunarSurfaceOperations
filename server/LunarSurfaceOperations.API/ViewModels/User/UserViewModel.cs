namespace LunarSurfaceOperations.API.ViewModels.User
{
    using JetBrains.Annotations;
    using MongoDB.Bson;

    public class UserViewModel
    {
        public ObjectId Id { [UsedImplicitly] get; init; }
        public string Username { [UsedImplicitly] get; init; }
    }
}