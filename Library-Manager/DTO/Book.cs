using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Manager.DTO
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image URL or Image File is required.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Rating is required. Rating must be between 0 and 5.")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Publication Date is required.")]
        [Display(Name = "Publication Date")]
        public DateTime PublicationDate { get; set; }

        public bool IsTaken { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}
