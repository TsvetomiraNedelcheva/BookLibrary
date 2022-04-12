namespace BookLibrary.Core.Services.ServiceModels
{
    public class HomePageBookServiceModel
    {
        public bool IsDeleted { get; set; }
        public bool IsAvailableToAddByUser { get; set; } = true;
        public IEnumerable<HomeBookServiceModel> Books { get; set; }
    }
}
