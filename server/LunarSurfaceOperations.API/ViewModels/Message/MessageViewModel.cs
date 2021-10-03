namespace LunarSurfaceOperations.API.ViewModels.Message
{
    using System.Collections.Generic;
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;
    using MongoDB.Bson;

    public class MessageViewModel
    {
        private readonly List<IMessageAttributeViewModel> _attributes = new();

        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public IReadOnlyCollection<IMessageAttributeViewModel> Attributes => this._attributes.AsReadOnly();

        public void AddAttribute(IMessageAttributeViewModel attribute)
        {
            if (attribute is not null)
                this._attributes.Add(attribute);
        }
    }
}