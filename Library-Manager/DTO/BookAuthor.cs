using System.ComponentModel.DataAnnotations;

namespace Library_Manager.DTO
{
    public class BookAuthor
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }

    }
}
