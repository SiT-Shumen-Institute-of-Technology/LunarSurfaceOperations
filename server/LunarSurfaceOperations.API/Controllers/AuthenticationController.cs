namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.Authentication;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("_auth")]
    public class AuthenticationController : ControllerBase
    {
        [NotNull]
        private readonly IUserService _userService;

        public AuthenticationController([NotNull] IUserService userService)
        {
            this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        
        [HttpPost("signup")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationInputModel registrationInputModel, CancellationToken cancellationToken)
        {
            if (registrationInputModel is null)
                return this.BadRequest(ValidationMessages.InvalidRequest);

            var userPrototype = new UserPrototype(registrationInputModel.Username, registrationInputModel.Email, registrationInputModel.Password);
            var createUser = await this._userService.CreateAsync(userPrototype, cancellationToken);
            if (createUser.Success == false)
                return this.BadRequest(createUser.ToString());

            return this.Ok();
        }
    }
}