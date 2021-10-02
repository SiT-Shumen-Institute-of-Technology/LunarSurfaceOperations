namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.Authentication;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [AllowAnonymous]
    [Route("_auth")]
    public class AuthenticationController : ControllerBase
    {
        [NotNull]
        private readonly IUserService _userService;

        [NotNull]
        private readonly IAuthenticationTokenFactory _tokenFactory;

        public AuthenticationController([NotNull] IUserService userService, [NotNull] IAuthenticationTokenFactory tokenFactory)
        {
            this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this._tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
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

        [HttpPost("signin")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginInputModel loginInputModel, CancellationToken cancellationToken)
        {
            if (loginInputModel is null)
                return this.BadRequest(ValidationMessages.InvalidRequest);

            var getAuthenticationData = await this._userService.GetAuthenticationDataAsync(loginInputModel.Username, loginInputModel.Password, cancellationToken);
            if (getAuthenticationData.Success is false)
                return this.BadRequest(getAuthenticationData.ToString());
                
            var claims = new Dictionary<string, string>(capacity: 1) { [ClaimTypes.Sid] = getAuthenticationData.Data?.ToString() };
            var generateToken = this._tokenFactory.GenerateToken(claims);
            if (generateToken.Success is false)
                return this.BadRequest(generateToken.ToString());

            var authenticationModel = new AuthenticationViewModel { Token = generateToken.Data };
            return this.Ok(authenticationModel);
        }
    }
}