﻿namespace BookLibrary.Core.Services
{
    public class BookServiceModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool IsAvailableToAddByUser { get; set; } = true;
    }
}
