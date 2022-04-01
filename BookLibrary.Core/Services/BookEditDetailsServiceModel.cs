using BookLibrary.Infrastructure.Data.Models.Enums;

namespace BookLibrary.Core.Services
{
    public class BookEditDetailsServiceModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public IEnumerable<AuthorImagesServiceModel> Authors { get; set; }
        public ICollection<GenreServiceModel> Genres { get; set; }
    }
}
