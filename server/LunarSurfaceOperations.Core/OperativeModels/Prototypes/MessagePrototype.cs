namespace LunarSurfaceOperations.Core.OperativeModels.Prototypes
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using MongoDB.Bson;

    public record MessagePrototype : IMessagePrototype
    {
        public MessagePrototype(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}