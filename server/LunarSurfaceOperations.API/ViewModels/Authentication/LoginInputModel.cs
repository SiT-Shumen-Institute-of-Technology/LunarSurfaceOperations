namespace LunarSurfaceOperations.API.ViewModels.Authentication
{
    using JetBrains.Annotations;

    public class LoginInputModel
    {
        public string Username { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}