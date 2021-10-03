namespace LunarSurfaceOperations.API.ViewModels.MessageAttributes
{
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;

    public class MessageStringAttributeInputModel : IMessageAttributeInputModel
    {
        public string AttributeName { get; [UsedImplicitly] set; }
        public string Value { get; [UsedImplicitly] set; }

        public IMessageAttributePrototype ConstructPrototype() => new MessageStringAttributePrototype(this.AttributeName, this.Value);
    }
}