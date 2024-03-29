﻿using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data;

namespace BookLibrary.Core.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext data;
        public AuthorService(ApplicationDbContext _data)
        {
            data = _data;
        }

        public AuthorDetailsServiceModel AuthorDetails(string id)
        {
            var authorBooks = data.Authors.Where(a => a.Id == id)
                .SelectMany(a => a.Books.Where(b => b.IsDeleted == false)).ToList();
            var bookList = new List<BookDetailsServiceModel>();
            foreach (var book in authorBooks)
            {
                var bookImage = data.Books.Where(b => b.Id == book.Id).Select(x=>x.BookImage.RemoteImageUrl).FirstOrDefault();
                var bookToAddToList = new BookDetailsServiceModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Image = bookImage,
                    Description = book.Description,
                    Pages = book.Pages,
                    Authors = book.Authors.Select(x => x.Name.ToString()).ToList(),
                    Genres = book.Genres.Select(x => x.Name.ToString()).ToList(),
                };

                bookList.Add(bookToAddToList);

            }

            var author = data.Authors
                .Where(a => a.Id == id)
                .Select(a => new AuthorDetailsServiceModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Image = a.AuthorImage.RemoteImageUrl,
                    Books = bookList
                }).FirstOrDefault();

            return author;
        }

        public AuthorsQueryServiceModel GetAllAuthors(string searchTerm, int currentPage, int authorsPerPage)
        {

            var authorQuery = this.data.Authors.AsQueryable();

            var totalAuthors = authorQuery.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                authorQuery = authorQuery.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            var authors = authorQuery
                .Where(a => a.Books.Any(b => b.IsDeleted == false))
                .Skip((currentPage - 1) * authorsPerPage)
                .Take(authorsPerPage)
                .OrderByDescending(b => b.Id)
                .Select(a => new AllAuthorsServiceModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Image = a.AuthorImage.RemoteImageUrl,
                })
                .ToList();

            return new AuthorsQueryServiceModel
            {
                TotalAuthors = totalAuthors,
                CurrentPage = currentPage,
                Authors = authors
            };
        }
    }
}
