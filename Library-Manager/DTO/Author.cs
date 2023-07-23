using System.ComponentModel.DataAnnotations;

namespace Library_Manager.DTO
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birth Year is required.")]
        public int BirthYear { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}
