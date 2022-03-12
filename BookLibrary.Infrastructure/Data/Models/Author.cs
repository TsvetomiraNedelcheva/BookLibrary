using System.ComponentModel.DataAnnotations;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Infrastructure.Data.Models
{
    public class Author
    {
        public Author()
        {
            Id = Guid.NewGuid().ToString();
            Books = new List<Book>();
            Genres = new List<Genre>();
        }

        [Key]
        public string Id { get; set; }

        [StringLength(MaxAuthorNameLength)]
        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Book> Books { get; set; }
        public ICollection<Genre> Genres { get; set; }


    }
}
