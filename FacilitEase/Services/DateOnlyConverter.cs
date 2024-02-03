using System.Text.Json;
using System.Text.Json.Serialization;

namespace FacilitEase.Services
{
    /// <summary>
    /// Converts a DateOnly object to and from JSON.
    /// </summary>
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        /// <summary>
        /// Reads and converts the JSON to type DateOnly.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type of object to convert.</param>
        /// <param name="options">The JsonSerializerOptions to use.</param>
        /// <returns>A DateOnly object value.</returns>
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

        /// <summary>
        /// Writes a DateOnly value to a Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">The JsonSerializerOptions to use.</param>
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