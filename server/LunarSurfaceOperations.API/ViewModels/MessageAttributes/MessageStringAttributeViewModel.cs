namespace LunarSurfaceOperations.API.ViewModels.MessageAttributes
{
    using JetBrains.Annotations;

    public class MessageStringAttributeViewModel : IMessageAttributeViewModel
    {
        public string Type => "string";
        public string AttributeName { [UsedImplicitly] get; init; }
        public string Value { [UsedImplicitly] get; init; }
    }
}