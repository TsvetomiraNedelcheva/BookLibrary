using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using CloudinaryDotNet;
using System.Linq;

namespace BookLibrary.Core.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Cloudinary cloudinary;
        public BookService(ApplicationDbContext _data, UserManager<ApplicationUser> _userManager, Cloudinary _cloudinary)
        {
            data = _data;
            userManager = _userManager;
            cloudinary = _cloudinary;
        }
        public BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage, List<Book> currentUserBooks)
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
                    Id = b.Id,
                    Title = b.Title,
                    Image = b.BookImage.RemoteImageUrl
                })
                .ToList();


            foreach (var book in books)
            {
                var userBook = currentUserBooks.Where(x => x.Id == book.Id).FirstOrDefault();
                if (userBook != null)
                {
                    book.IsAvailableToAddByUser = false;
                }
            }

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

        public async Task RemoveFromBookList(string bookId, string userId)
        {
            var bookToRemove = await data.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            var user = await data.ApplicationUsers.Include(x => x.Books).FirstOrDefaultAsync(a => a.Id == userId);
            user.Books.Remove(bookToRemove);
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
                    Image = b.BookImage.RemoteImageUrl,
                    Authors = authorsList,
                    Genres = genresList,
                    Publisher = b.Publisher.Name
                }).FirstOrDefault();

            return book;
        }

        public async Task Edit(string id, string title, string description, IFormFile image, int pages, string publisher,
            string authorsInput, List<string> genres)
        {
            var authorsNamesList = authorsInput.Split(","); //array of the names

            var bookData = data.Books.Include(bi => bi.BookImage)
                                     .Include(x => x.Genres).FirstOrDefault(x => x.Id == id); //the book
            var bookAuthors = data.Books.Where(x => x.Id == id).SelectMany(x => x.Authors).ToList(); //bookAuthors

            var authorsList = new List<Author>(); //list of authors to be mapped              

            var authorBooksToRemove = bookAuthors.Select(x => x.Name).Except(authorsNamesList).ToList();
            foreach (var author in authorBooksToRemove)
            {
                var authorToEdit = data.Authors.Where(x => x.Name == author).Include(x => x.Books).FirstOrDefault();
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

                    };
                    newAuthor.Books.Add(bookData);
                    authorsList.Add(newAuthor);
                    data.Authors.Add(newAuthor); //adding the author in the db
                }
                else
                {
                    var authorBooks = data.Authors.Where(x => x.Name == authorName).SelectMany(x => x.Books).ToList();
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
                var genreEnum = Enum.Parse<GenreType>(editedGenre);
                var newGenre = new Genre()
                {
                    Name = genreEnum
                };
                genresList.Add(newGenre);
            }
            genresList = genresList.Except(bookGenres).ToList();

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

            if (image != null)
            {
                var resultUrl = await Cloud.Cloud.UploadAsync(this.cloudinary, image); //returns link of the image

                bookData.BookImage.RemoteImageUrl = resultUrl;
            }

            bookData.Title = title;
            bookData.Description = description;
            bookData.Pages = pages;
            bookData.Publisher = newPublisher;
            bookData.Genres = genresList;

            await data.SaveChangesAsync();
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
                  Image = b.BookImage.RemoteImageUrl,
                  Authors = b.Authors.Select(x => x.Name.ToString()).ToList(),
                  Genres = b.Genres.Select(x => x.Name.ToString()).ToList(),
                  Users = b.Reviews.Select(x => x.User).ToList(),
                  Reviews = b.Reviews,
                  Publisher = b.Publisher.Name
              }).FirstOrDefault();

            return book;
        }

        public async Task Add(string title, string description, int pages, IFormFile image, string publisher, IEnumerable<AuthorImagesServiceModel> authors, ICollection<GenreType> genres)
        {
            var genresList = new List<Genre>();
            foreach (var item in genres)
            {
                genresList.Add(new Genre { Name = item });
            }

            var newBook = new Book
            {
                Title = title,
                Description = description,
                Pages = pages,
                Publisher = new Publisher { Name = publisher },
                Genres = genresList,
            };

            foreach (var inputAuthor in authors)
            {
                var author = await data.Authors.FirstOrDefaultAsync(a => a.Name == inputAuthor.Name);
                if (author == null)
                {
                    var resultUrl = await Cloud.Cloud.UploadAsync(this.cloudinary, inputAuthor.AuthorImage); //returns link of the image

                    author = new Author()
                    {
                        Name = inputAuthor.Name
                    };
                   
                    author.AuthorImage = new AuthorImage()
                    {
                        RemoteImageUrl = resultUrl
                    };

                    await data.AuthorImages.AddAsync(author.AuthorImage);
                    await data.Authors.AddAsync(author);
                }

                newBook.Authors.Add(author);
            }

            if (image != null)
            {
                var resultUrl = await Cloud.Cloud.UploadAsync(this.cloudinary, image); //returns link of the image
                newBook.BookImage = new BookImage
                {
                    Book = newBook,
                    RemoteImageUrl = resultUrl,
                    BookId = newBook.Id
                };
            }

            await data.Books.AddAsync(newBook);
            await data.SaveChangesAsync();
        }

        public void LeaveReview(string bookId, string userId, string content)
        {
            var book = data.Books.Where(b => b.Id == bookId).FirstOrDefault();
            book.Reviews.Add(new Review
            {
                BookId = book.Id,
                UserId = userId,
                Text = content
            });

            data.SaveChanges();
        }
    }
}
