namespace LunarSurfaceOperations.Core.OperativeModels.Prototypes
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Data.Models;

    public record MessageStringAttributePrototype : IMessageAttributePrototype
    {
        public string AttributeName { get; }
        public string Value { get; }

        public MessageStringAttributePrototype(string attributeName, string value)
        {
            this.AttributeName = attributeName;
            this.Value = value;
        }

        public IMessageAttribute Materialize()
            => new MessageStringAttribute
            {
                AttributeName = this.AttributeName,
                Value = this.Value
            };
    }
}