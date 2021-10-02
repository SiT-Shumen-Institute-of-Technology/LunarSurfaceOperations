namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.Message;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;

    [ApiController]
    [Route("_messages")]
    public class MessagesController : ControllerBase
    {
        [NotNull]
        private readonly IMessageService _messageService;

        public MessagesController([NotNull] IMessageService messageService)
        {
            this._messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromQuery] ObjectId workspaceId, [FromBody] MessageInputModel inputModel, CancellationToken cancellationToken)
        {
            if (inputModel is null)
                return this.BadRequest(ValidationMessages.InvalidRequest);

            var messagePrototype = new MessagePrototype(inputModel.Text);
            var createMessage = await this._messageService.CreateAsync(workspaceId, messagePrototype, cancellationToken);
            if (createMessage.Success is false)
                return this.BadRequest(createMessage.ToString());

            return this.Ok();
        }
    }
}