using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Books = new List<Book>();
        }
        [Required]
        [StringLength(UserFirstNameMaxLength)]
        public string FisrtName { get; set; }

        [Required]
        [StringLength(UserLastNameMaxLength)]
        public string LastName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
