using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Infrastructure.Data.Models.Enums;
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

            var bookQuery = this.data.Books.Where(b => b.IsDeleted == false).AsQueryable();

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
                })
                .ToList();

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
            var book = await data.Books.Where(b => b.IsDeleted == false).FirstOrDefaultAsync(b => b.Id == bookId);
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

        public BookEditDetailsServiceModel EditDetails(string id)
        {
            var authorsList = new List<AuthorImagesServiceModel>();
            var bookAuthorsDb = this.data.Books.Where(x => x.Id == id).SelectMany(x => x.Authors).ToList();
            foreach (var author in bookAuthorsDb)
            {
                authorsList.Add(new AuthorImagesServiceModel
                {
                    Name = author.Name,
                    AuthorImageUrl = author.ImageUrl
                });
            }

            var genresList = new List<GenreServiceModel>();
            var genresDb = this.data.Books.Where(x => x.Id == id).SelectMany(x => x.Genres).ToList();

            foreach (var genre in genresDb)
            {
                genresList.Add(new GenreServiceModel
                {
                    Name = genre.Name.ToString(),
                });
            }

            var book = this.data.Books
                .Where(b => b.Id == id)
                .Select(b => new BookEditDetailsServiceModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Pages = b.Pages,
                    ImageUrl = b.ImageUrl,
                    Authors = authorsList,
                    Genres = genresList,
                    Publisher = b.Publisher.Name
                }).FirstOrDefault();

            return book;
        }

        public void Edit(string id, string title, string description, string imageURL, int pages, string publisher,
            string authorsInput, List<string> genres)
        {
            var authorsNamesList = authorsInput.Split(","); //array of the names

            var bookData = data.Books.FirstOrDefault(x => x.Id == id); //the book
            var bookAuthors = data.Books.Where(x => x.Id == id).SelectMany(x => x.Authors).ToList(); //bookAuthors

            var authorsList = new List<Author>(); //list of authors to be mapped              

            var authorBooksToRemove = bookAuthors.Select(x => x.Name).Except(authorsNamesList).ToList();
            foreach (var author in authorBooksToRemove)
            {
                var authorToEdit = data.Authors.Where(x => x.Name == author).Include(x=>x.Books).FirstOrDefault();
                authorToEdit.Books.Remove(bookData);
            }

            foreach (var authorName in authorsNamesList)
            {
                //finding the author w that name
                var author = bookAuthors.Where(a => a.Name == authorName).FirstOrDefault();

                if (author == null)
                {
                    var newAuthor = new Author()
                    {
                        Name = authorName,
                        ImageUrl = "" // ?

                    };
                    newAuthor.Books.Add(bookData);
                    authorsList.Add(newAuthor);
                    data.Authors.Add(newAuthor); //adding the author in the db
                }
                else
                {
                    var authorBooks = data.Authors.Where(x=>x.Name == authorName).SelectMany(x => x.Books).ToList();
                    if (!authorBooks.Contains(bookData))
                    {
                        authorBooks.Add(bookData);
                    }
                }
            }


            var bookGenres = data.Books.Where(x => x.Id == id).SelectMany(x => x.Genres).ToList();
            var genresList = new List<Genre>();


            foreach (var editedGenre in genres)
            {
                if (editedGenre == GenreType.Classics.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Classics
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.Fiction.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Fiction
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.NonFiction.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.NonFiction
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.Science.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Science
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.Fantasy.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Fantasy
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.Romance.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Romance
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.Thriller.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Thriller
                    };
                    genresList.Add(newGenre);
                }
                else if (editedGenre == GenreType.Biography.ToString())
                {
                    var newGenre = new Genre()
                    {
                        Name = GenreType.Biography
                    };
                    genresList.Add(newGenre);
                }
            }


            var bookPublisher = data.Books.Where(x => x.Id == id).Select(x => x.Publisher).FirstOrDefault();
            Publisher newPublisher;
            if (publisher != bookPublisher.Name || bookPublisher == null)
            {
                newPublisher = new Publisher()
                {
                    Name = publisher
                };
                newPublisher.Books.Add(bookData);
                foreach (var author in authorsList)
                {
                    newPublisher.Authors.Add(author);
                }
            }
            else
            {
                newPublisher = bookPublisher;
            }

            bookData.Title = title;
            bookData.Description = description;
            bookData.Pages = pages;
            bookData.ImageUrl = imageURL;
            bookData.Publisher = newPublisher;
            //  bookData.Authors = authorsList;
            bookData.Genres = genresList;

            data.SaveChanges();
        }

        public void Delete(string id)
        {
            var bookToDelete = data.Books.FirstOrDefault(b => b.Id == id);
            bookToDelete.IsDeleted = true;
            data.SaveChanges();

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
                  ImageUrl = b.ImageUrl,
                  Authors = b.Authors.Select(x=>x.Name.ToString()).ToList(),
                  Genres = b.Genres.Select(x=>x.Name.ToString()).ToList(),
                  Publisher = b.Publisher.Name
              }).FirstOrDefault();

            return book;
        }
    }
}
