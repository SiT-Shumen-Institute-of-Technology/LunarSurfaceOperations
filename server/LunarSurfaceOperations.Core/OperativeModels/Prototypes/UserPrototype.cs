namespace LunarSurfaceOperations.Core.OperativeModels.Prototypes
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public record UserPrototype : IUserPrototype
    {
        public UserPrototype(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }

        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
    }
}