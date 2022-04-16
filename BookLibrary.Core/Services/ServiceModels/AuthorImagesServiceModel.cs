using Microsoft.AspNetCore.Http;

namespace BookLibrary.Core.Services
{
    public class AuthorImagesServiceModel
    {
        public string Name { get; set; }
        public IFormFile AuthorImage { get; set; }
    }
}
