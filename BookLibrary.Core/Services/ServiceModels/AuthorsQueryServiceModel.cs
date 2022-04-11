using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Core.Services.ServiceModels
{
    public class AuthorsQueryServiceModel
    {
        public string Id { get; set; }

        public const int AuthorsPerPage = 3;
        public int CurrentPage { get; set; } = 1;
        public int TotalAuthors { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public IEnumerable<AllAuthorsServiceModel> Authors { get; set; }
    }
}
