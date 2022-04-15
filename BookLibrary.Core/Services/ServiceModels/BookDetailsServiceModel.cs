using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services
{
    public class BookDetailsServiceModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
