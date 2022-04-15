using BookLibrary.Core.Services;

namespace BookLibrary.Models.Users
{
    public class MyBooksViewModel
    {
        public string UserName { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<BookServiceModel> Books { get; set; }

        public const int BooksPerPage = 4;
        public int CurrentPage { get; set; } = 1;
        public int TotalBooks { get; set; }
    }
}
