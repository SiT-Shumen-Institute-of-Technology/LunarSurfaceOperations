namespace LunarSurfaceOperations.API.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.Factories.Contracts.MessageAttributes;
    using LunarSurfaceOperations.API.ViewModels.Message;
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Utilities;
    using Quantum.DMS.Utilities;

    public class MessageFactory : IMessageFactory
    {
        private readonly IReadOnlyCollection<IMessageAttributeLayoutProcessor> _attributeProcessors;
        private readonly IUserFactory _userFactory;

        public MessageFactory([CanBeNull] IEnumerable<IMessageAttributeLayoutProcessor> attributeProcessors, [NotNull] IUserFactory userFactory)
        {
            this._attributeProcessors = attributeProcessors.OrEmptyIfNull().IgnoreNullValues().ToList().AsReadOnly();
            this._userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
        }

        public MessageViewModel ToViewModel(IMessageLayout messageLayout)
        {
            if (messageLayout is null)
                return null;

            var messageViewModel = new MessageViewModel
            {
                Id = messageLayout.Id,
                Text = messageLayout.Text,
                Author = this._userFactory.ToViewModel(messageLayout.Author),
                Timestamp = messageLayout.Timestamp.GetUnixTimeSeconds()
            };

            foreach (var attributeLayout in messageLayout.Attributes.OrEmptyIfNull().IgnoreNullValues())
                if (this.TryProcessAttribute(attributeLayout, out var attributeViewModel))
                    messageViewModel.AddAttribute(attributeViewModel);

            return messageViewModel;
        }

        private bool TryProcessAttribute(IMessageAttributeLayout attributeLayout, out IMessageAttributeViewModel attributeViewModel)
        {
            attributeViewModel = null;
            if (attributeLayout is null)
                return false;

            foreach (var messageAttributeProcessor in this._attributeProcessors)
            {
                if (messageAttributeProcessor.CanProcess(attributeLayout) is false)
                    continue;

                attributeViewModel = messageAttributeProcessor.Process(attributeLayout);
                return attributeViewModel is not null;
            }

            return false;
        }
    }
}