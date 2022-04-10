namespace BookLibrary.Core.Services
{
    public class BookQueryServiceModel
    {
        public int CurrentPage { get; set; }
        public int BooksPerPage { get; set; }
        public int TotalBooks { get; set; }

        public bool IsAvailableToAddByUser { get; set; } = true;
        public IEnumerable<BookServiceModel> Books { get; set; }
    }
}
