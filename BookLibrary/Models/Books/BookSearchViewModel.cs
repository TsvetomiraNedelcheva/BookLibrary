using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Books
{
    public class BookSearchViewModel
    {
        public IEnumerable<string> Titles { get; set; }

        public string GenreType { get; set; }
        public IEnumerable<string> GenreTypes { get; set; }
        public IEnumerable<string> AuthorNames { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public IEnumerable<AllBooksViewModel> Books { get; set; }
    }
}
