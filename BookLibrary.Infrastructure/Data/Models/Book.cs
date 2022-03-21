using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Infrastructure.Data.DataConstants;

namespace BookLibrary.Infrastructure.Data.Models
{
    public class Book
    {
        public Book()
        {
            Id = Guid.NewGuid().ToString();
            Authors = new List<Author>();
            Genres = new List<Genre>();
            Users = new List<ApplicationUser>();
            Reviews = new List<Review>();
        }

        [Key]
        public string Id { get; set; } 
        
        [StringLength(MaxBookTitleLength)]
        [Required]
        public string Title { get; set; }

        [StringLength(MaxBookDescriptionLength)]
        [Required]
        public string Description { get; set; }
        public int Pages { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string PublisherId { get; set; }

        [ForeignKey(nameof(PublisherId))]
        public Publisher Publisher { get; set; }

        public bool IsDeleted { get; set; }
        
        public ICollection<Author> Authors { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Review> Reviews { get; set; }


    }
}
