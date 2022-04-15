namespace BookLibrary.Models.Books
{
    public class HomePageBooksViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public string Image { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailableToAddByUser { get; set; }
        public IEnumerable<AllBooksViewModel> Books { get; set; }
    }
}
