﻿namespace BookLibrary.Core.Services.ServiceModels
{
    public class AuthorDetailsServiceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<BookDetailsServiceModel> Books { get; set; }
    }
}
