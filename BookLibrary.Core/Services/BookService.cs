using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddToMyBooks(string bookId, string userId)
        {
            var book = await data.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            var user = await data.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == userId);

            if (book == null || user == null)
            {
                return;
            }
            if (user.Books.Contains(book))
            {
                return;
            }

            user.Books.Add(book);
            await data.SaveChangesAsync();
        }

        public BookDetailsServiceModel Details(string id)
        {
            var book = this.data.Books
                .Where(b => b.Id == id)
                .Select(b => new BookDetailsServiceModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Pages = b.Pages,
                    ImageUrl = b.ImageUrl
                }).FirstOrDefault();

            foreach (var inputAuthor in book.Authors) //because book is a service model
            {
                var author = data.Authors
                    .Where(a => a.Name == inputAuthor.Name)
                    .Select(a => new AuthorImagesServiceModel
                    {
                        Name = inputAuthor.Name,
                        AuthorImageUrl = inputAuthor.AuthorImageUrl,
                    }).FirstOrDefault();

                var authorsList = new List<AuthorImagesServiceModel>();
                authorsList.Add(author);

                book.Authors = authorsList;
            }

            if (book.Publisher == null)
            {
                var publisher = new Publisher()
                {
                    Name = book.Publisher,
                };

                data.Publishers.Add(publisher);
                book.Publisher = publisher.Name;
            }
            else
            {
                var publisher = data.Publishers.Where(p => p.Name == book.Publisher).FirstOrDefault();
                book.Publisher = publisher.Name;
            }

            foreach (var inputGenre in book.Genres)
            {
                var genre = data.Genres
                    .Where(g => g.Name.ToString() == inputGenre.Name)
                    .Select(g => new GenreServiceModel
                    {
                       Name=g.Name.ToString(),

                    }).FirstOrDefault();

                var genresList = new List<GenreServiceModel>();
                genresList.Add(genre);

                book.Genres = genresList;
            }

            return book;
        }

        public bool Edit(string id, string title, string description, string imageURL, int pages, Publisher publisher, ICollection<Author> authors, ICollection<Genre> genres)
        {
            var bookData = data.Books.Find(id);

            bookData.Title = title;
            bookData.Description = description;
            bookData.Authors = authors;
            bookData.Genres = genres;
            bookData.Pages = pages;
            bookData.ImageUrl = imageURL;
            bookData.Publisher = publisher;

            data.SaveChanges();
            return true;
        }
    }
}
