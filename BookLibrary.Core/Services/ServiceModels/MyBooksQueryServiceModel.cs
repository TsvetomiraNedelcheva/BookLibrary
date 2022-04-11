using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services.ServiceModels
{
    public  class MyBooksQueryServiceModel
    {
        public string UserName { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public int CurrentPage { get; set; }
        public int BooksPerPage { get; set; }
        public int TotalBooks { get; set; }
        public IEnumerable<BookServiceModel> Books { get; set; }
    }
}
