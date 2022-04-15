using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Books
{
    public class AuthorImagesViewModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        public string? AuthorImageUrl { get; set; }

        public IFormFile AuthorImage { get; set; }
    }
}
