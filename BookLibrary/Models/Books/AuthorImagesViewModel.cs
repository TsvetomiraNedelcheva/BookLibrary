using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Books
{
    public class AuthorImagesViewModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [Url]
        public string AuthorImageUrl { get; set; }
    }
}
