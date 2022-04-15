namespace BookLibrary.Infrastructure.Data.Models
{
    public class BookImage
    {
        public BookImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string BookId { get; set; }
        public  Book Book { get; set; }
        public string RemoteImageUrl { get; set; }
    }
}
