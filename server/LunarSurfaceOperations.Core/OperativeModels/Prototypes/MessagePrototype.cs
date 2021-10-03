namespace LunarSurfaceOperations.Core.OperativeModels.Prototypes
{
    using System.Collections.Generic;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public record MessagePrototype : IMessagePrototype
    {
        private readonly List<IMessageAttributePrototype> _attributes = new();

        public MessagePrototype(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
        public IReadOnlyCollection<IMessageAttributePrototype> Attributes => this._attributes.AsReadOnly();

        public void AddAttribute(IMessageAttributePrototype attributePrototype)
        {
            if (attributePrototype is null)
                return;

            this._attributes.Add(attributePrototype);
        }
    }
}