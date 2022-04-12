namespace BookLibrary.Core.Services.ServiceModels
{
    public class HomePageBookServiceModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailableToAddByUser { get; set; } = true;
        public IEnumerable<HomeBookServiceModel> Books { get; set; }
    }
}
