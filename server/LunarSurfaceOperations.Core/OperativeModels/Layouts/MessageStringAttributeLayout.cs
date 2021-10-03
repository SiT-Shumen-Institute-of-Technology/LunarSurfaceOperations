namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public record MessageStringAttributeLayout : IMessageAttributeLayout
    {
        public string AttributeName { get; }
        public string Value { get; }

        public MessageStringAttributeLayout(string attributeName, string value)
        {
            this.AttributeName = attributeName;
            this.Value = value;
        }
    }
}