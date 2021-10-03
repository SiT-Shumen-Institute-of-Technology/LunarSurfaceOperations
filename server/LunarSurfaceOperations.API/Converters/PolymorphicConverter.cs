namespace LunarSurfaceOperations.API.Converters
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public abstract class PolymorphicConverter<T> : JsonConverter<T>
    {
        protected virtual string TypePropertyName => "type";

        protected abstract Type GetType(JsonElement element);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Get the `type` value by parsing the JSON string into a JsonDocument.
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            jsonDocument.RootElement.TryGetProperty(this.TypePropertyName, out var typeNameElement);

            var json = GetJsonText(options, jsonDocument);
            var returnType = this.GetType(typeNameElement);
            if (returnType is null)
                throw new InvalidOperationException("The received JSON cannot be deserialized to any known type.");

            try
            {
                // Deserialize the JSON to the specified type.
                return (T) JsonSerializer.Deserialize(json, returnType, options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid JSON in request.", ex);
            }
        }

        private static string GetJsonText(JsonSerializerOptions options, JsonDocument jsonDocument)
        {
            if (jsonDocument is null)
                return null;

            // get the JSON text that was read by the JsonDocument
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Encoder = options?.Encoder });

            jsonDocument.WriteTo(writer);
            writer.Flush();

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, typeof(T), options);
    }
}