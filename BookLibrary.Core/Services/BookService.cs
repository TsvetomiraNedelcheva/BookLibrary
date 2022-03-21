using BookLibrary.Infrastructure.Data;
using System.Linq;

namespace BookLibrary.Core.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext data;
        public BookService(ApplicationDbContext _data)
        {
            data = _data;
        }
        public BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage)
        {

            var bookQuery = this.data.Books.AsQueryable();

            var totalBooks = bookQuery.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                bookQuery = bookQuery.Where(b => b.Title.ToLower().Contains(searchTerm.ToLower()));
            }
            var books = bookQuery
                .Skip((currentPage - 1) * booksPerPage)
                .Take(booksPerPage)
                .OrderByDescending(b => b.Id)
                .Select(b => new BookServiceModel
                {
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    //Authors = (IEnumerable<AuthorImagesViewModel>)b.Authors,
                    //Genres = (ICollection<GenreType>)b.Genres
                })
                .ToList();

            //if (!string.IsNullOrWhiteSpace(genre))
            //{
            //    bookQuery = bookQuery.Where(b => b.Genres.Select(g => g.Name).ToString() == genre);
            //}

            //var bookGenres = data
            //    .Genres
            //    .Select(g => g.Name.ToString()) 
            //    .ToList();

            // GenreTypes = bookGenres;
            // SearchTerm = query.SearchTerm
            //query.TotalBooks = totalBooks;
            //query.Books = books;

            return new BookQueryServiceModel
            {
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                BooksPerPage = booksPerPage,
                Books = books
            };
        }

        //public IEnumerable<BookServiceModel> MyBooks(string userId)
        //{
           
        //}
    }
}
