namespace LunarSurfaceOperations.API.Factories.MessageAttributes
{
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;

    public class MessageStringAttributeLayoutProcessor : BaseMessageAttributeLayoutProcessor<MessageStringAttributeLayout>
    {
        protected override IMessageAttributeViewModel Process(MessageStringAttributeLayout entity)
        {
            if (entity is null)
                return null;

            return new MessageStringAttributeViewModel
            {
                AttributeName = entity.AttributeName,
                Value = entity.Value
            };
        }
    }
}