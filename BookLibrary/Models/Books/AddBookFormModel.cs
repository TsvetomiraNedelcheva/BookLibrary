using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Books
{
    public class AddBookFormModel
    { 
        public string Title { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }

        [Display(Name= "Image URL")]
        public string ImageUrl { get; set; }
        public string Publisher { get; set; }
        public string Authors { get; set; }

        [Display(Name = "Genre")]
        public string GenreId { get; set; }
        public IEnumerable<BookGenreViewModel> Genres { get; set; }
    }
}
