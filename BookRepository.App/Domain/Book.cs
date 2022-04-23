using System.Text.Json.Serialization;

namespace BookRepository.App.Domain
{
    public class Book : IEquatable<Book>
    {
        [JsonPropertyName("id")]
        [JsonPropertyOrder(0)]
        public string Id { get; set; }

        [JsonPropertyName("author")]
        [JsonPropertyOrder(1)]
        public string Author { get; set; }

        [JsonPropertyName("title")]
        [JsonPropertyOrder(2)]
        public string Title { get; set; }
        
        [JsonPropertyName("genre")]
        [JsonPropertyOrder(3)]
        public string Genre { get; set; }

        [JsonPropertyName("price")]
        [JsonPropertyOrder(4)]
        [JsonNumberHandling(JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString)]
        public decimal Price { get; set; }

        [JsonPropertyName("publish_date")]
        [JsonPropertyOrder(5)]
        public DateTime Published { get; set; }
        
        [JsonPropertyName("description")]
        [JsonPropertyOrder(6)]
        public string Description { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Book other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(Book? other)
        {
            if(other == null) return false;

            if (ReferenceEquals(other, this)) return true;

            return other.Id == Id
                && other.Author == Author
                && other.Title == Title
                && other.Genre == Genre
                && other.Published == Published
                && other.Description == Description
                && other.Price == Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Author, Title, Genre, Published, Description, Price);
        }
    }
}
