using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookRepository.App.Converters
{
    public class DecimalFormatConverter : JsonConverter<decimal>
    {
		private readonly string Format;
		public DecimalFormatConverter(string format)
		{
			Format = format;
		}
		public override void Write(Utf8JsonWriter writer, decimal number, JsonSerializerOptions options)
		{
			writer.WriteStringValue(number.ToString(Format, CultureInfo.InvariantCulture));
		}

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return decimal.Parse(reader.GetString(), CultureInfo.InvariantCulture);
        }

    }
}
