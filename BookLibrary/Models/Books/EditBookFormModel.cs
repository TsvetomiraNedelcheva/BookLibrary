using BookLibrary.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Models.Books
{
    public class EditBookFormModel
    {
        [Required]
        [StringLength(MaxBookTitleLength, MinimumLength = MinBookTitleLength,
            ErrorMessage = "The field must be between {2} and {1} characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(MaxBookDescriptionLength, MinimumLength = MinBookDescriptionLength,
            ErrorMessage = "The field must be between {2} and {1} characters.")]
        public string Description { get; set; }

        [Required]
        [Range(MinPagesValue, MaxPagesValue, ErrorMessage = "The value must be between {1} and {2}.")]
        public int Pages { get; set; }

        [Required]
        [Display(Name = "Image Link")]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(MaxPublisherNameLength, MinimumLength = MinPublisherNameLength,
            ErrorMessage = "The field must be between {2} and {1} characters.")]
        public string Publisher { get; set; }
        public string Authors { get; set; }
        public ICollection<GenreType> Genres { get; set; }
    }
}
