namespace LunarSurfaceOperations.Core.OperativeModels.Prototypes
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public record UserPrototype : IUserPrototype
    {
        public UserPrototype(string name, string email, string password)
        {
            this.Username = name;
            this.Email = email;
            this.Password = password;
        }

        public string Username { get; }
        public string Email { get; }
        public string Password { get; }
    }
}