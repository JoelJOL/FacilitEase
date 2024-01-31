using System.Text.Json.Serialization;
using System.Text.Json;

namespace FacilitEase.Services
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // Parse the object and ignore the "dayOfWeek" property
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    var root = doc.RootElement;
                    int year = root.GetProperty("year").GetInt32();
                    int month = root.GetProperty("month").GetInt32();
                    int day = root.GetProperty("day").GetInt32();

                    return new DateOnly(year, month, day);
                }
            }

            throw new JsonException($"Unable to parse date from {reader.GetString()}");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("year", value.Year);
            writer.WriteNumber("month", value.Month);
            writer.WriteNumber("day", value.Day);
            writer.WriteEndObject();
        }
    }


}
