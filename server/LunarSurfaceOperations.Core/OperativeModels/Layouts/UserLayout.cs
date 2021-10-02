namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record UserLayout : BaseLayout, IUserLayout
    {
        public UserLayout(ObjectId id, string username, string email)
            : base(id)
        {
            this.Username = username;
            this.Email = email;
        }

        public string Username { get; }
        public string Email { get; }
    }
}