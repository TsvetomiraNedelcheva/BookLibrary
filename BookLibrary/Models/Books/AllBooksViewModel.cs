using BookLibrary.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Models.Books
{
    public class AllBooksViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(MaxBookTitleLength, MinimumLength = MinBookTitleLength,
            ErrorMessage = "The field must be between {2} and {1} characters.")]
        public string Title { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; } 
        public IEnumerable<AuthorImagesViewModel> Authors { get; set; }
        public ICollection<GenreType> Genres { get; set; }
    }
}
