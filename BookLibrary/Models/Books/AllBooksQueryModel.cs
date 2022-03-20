using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Books
{
    public class AllBooksQueryModel
    {
        public const int BooksPerPage = 6;
        public int CurrentPage { get; set; } = 1;
        public int TotalBooks { get; set; }
        public IEnumerable<string> Titles { get; set; }
        public string GenreType { get; set; }
        public IEnumerable<string> GenreTypes { get; set; }
        public IEnumerable<string> AuthorNames { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public IEnumerable<AllBooksViewModel> Books { get; set; }
    }
}
