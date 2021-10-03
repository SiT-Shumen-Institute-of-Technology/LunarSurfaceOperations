namespace LunarSurfaceOperations.API.Converters
{
    using System;
    using System.Text.Json;
    using LunarSurfaceOperations.API.ViewModels.MessageAttributes;

    public class MessageAttributeInputModelConverter : PolymorphicConverter<IMessageAttributeInputModel>
    {
        protected override Type GetType(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.String)
                return null;

            return element.GetString() switch
            {
                "string" => typeof(MessageStringAttributeInputModel),
                _ => throw new InvalidOperationException()
            };
        }
    }
}