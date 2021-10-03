namespace LunarSurfaceOperations.API.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class PolymorphicSerializer<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => default;

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}