namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public record UserLayout : IUserLayout
    {
        public UserLayout(string username, string email)
        {
            this.Username = username;
            this.Email = email;
        }

        public string Username { get; }
        public string Email { get; }
    }
}