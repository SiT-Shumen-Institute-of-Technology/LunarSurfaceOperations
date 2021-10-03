namespace LunarSurfaceOperations.API.ViewModels.Message
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;
    using LunarSurfaceOperations.API.ViewModels.User;
    using MongoDB.Bson;

    public class MessageViewModel
    {
        private readonly List<IMessageAttributeViewModel> _attributes = new();

        public ObjectId Id { [UsedImplicitly] get; init; }
        public string Text { [UsedImplicitly] get; init; }
        public IReadOnlyCollection<IMessageAttributeViewModel> Attributes => this._attributes.AsReadOnly();
        public UserViewModel Author { [UsedImplicitly] get; init; }
        public long Timestamp { [UsedImplicitly] get; init; }

        public void AddAttribute(IMessageAttributeViewModel attribute)
        {
            if (attribute is not null)
                this._attributes.Add(attribute);
        }
    }
}