using Library_Manager.DTO;

namespace Library_Manager.ViewModels
{
    public class BookAuthorsViewModel
    {
        public List<Author> Authors { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
    }
}
