using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookRepository.App.Domain
{
    public class Book : IEquatable<Book>
    {
        [JsonPropertyName("id")]
        [JsonPropertyOrder(0)]
        [NotMapped]
        public string PresentedId { get; set; } = null!;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("author")]
        [JsonPropertyOrder(1)]
        [Required]
        public string Author { get; set; } = null!;

        [JsonPropertyName("title")]
        [JsonPropertyOrder(2)]
        [Required]
        public string Title { get; set; } = null!;

        [JsonPropertyName("genre")]
        [JsonPropertyOrder(3)]
        [Required]
        public string Genre { get; set; } = null!;

        [JsonPropertyName("price")]
        [JsonPropertyOrder(4)]
        [JsonNumberHandling(JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString)]
        [Required]
        public decimal Price { get; set; }

        [JsonPropertyName("publish_date")]
        [JsonPropertyOrder(5)]
        [Required]
        public DateTime Published { get; set; }

        [JsonPropertyName("description")]
        [JsonPropertyOrder(6)]
        [Required]
        public string Description { get; set; } = null!;

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
            if (other == null) return false;

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
