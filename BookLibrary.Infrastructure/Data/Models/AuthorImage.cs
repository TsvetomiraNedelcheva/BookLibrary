namespace BookLibrary.Infrastructure.Data.Models
{
    public class AuthorImage
    {
        public AuthorImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string AuthorId { get; set; }
        public Author Author { get; set; }
        public string RemoteImageUrl { get; set; }
    }
}
