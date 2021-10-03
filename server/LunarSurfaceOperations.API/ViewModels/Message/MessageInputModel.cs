namespace LunarSurfaceOperations.API.ViewModels.Message
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;

    public class MessageInputModel
    {
        public string Text { get; [UsedImplicitly] set; }
        public IEnumerable<IMessageAttributeInputModel> Attributes { get; [UsedImplicitly] set; }
    }
}