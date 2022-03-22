using BookLibrary.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Models.Users
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; } 

        [Required]
        [StringLength(UserFirstNameMaxLength)]
        public string FisrtName { get; set; }

        [Required]
        [StringLength(UserLastNameMaxLength)]
        public string LastName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
