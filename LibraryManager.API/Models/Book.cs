namespace LibraryManager.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsTaken { get; set; }
    }
}
