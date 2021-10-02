namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.ViewModels.Message;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using Quantum.DMS.Utilities;

    [ApiController]
    [Route("_messages")]
    public class MessagesController : ControllerBase
    {
        [NotNull]
        private readonly IMessageService _messageService;

        [NotNull]
        private readonly IMessageFactory _messageFactory;

        public MessagesController([NotNull] IMessageService messageService, [NotNull] IMessageFactory messageFactory)
        {
            this._messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            this._messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] ObjectId workspaceId, CancellationToken cancellationToken)
        {
            var getMessages = await this._messageService.GetManyAsync(workspaceId, cancellationToken);
            if (getMessages.Success is false)
                return this.BadRequest(getMessages.ToString());

            return this.Ok(getMessages.Data.OrEmptyIfNull().IgnoreNullValues().Select(this._messageFactory.ToViewModel));
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