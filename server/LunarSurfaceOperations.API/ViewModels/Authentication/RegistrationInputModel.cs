namespace LunarSurfaceOperations.API.ViewModels.Authentication
{
    using JetBrains.Annotations;

    public class RegistrationInputModel
    {
        public string Username { get; [UsedImplicitly] set; }
        public string Email { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}