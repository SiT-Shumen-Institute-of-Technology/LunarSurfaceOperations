namespace LunarSurfaceOperations.API.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using MongoDB.Bson;

    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value) || ObjectId.TryParse(value, out var id) == false)
                return default;

            return id;
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue(value.ToString());
        }
    }
}