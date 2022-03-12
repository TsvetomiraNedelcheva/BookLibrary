using System.ComponentModel.DataAnnotations;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Infrastructure.Data.Models
{
    public class Genre
    {
        public Genre()
        {
            Id = Guid.NewGuid().ToString();
            Books = new List<Book>();
            Authors = new List<Author>();
        }

        [Key]
        public string Id { get; set; }

        [StringLength(MaxGenreNameLength)]
        [Required]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}
