namespace WebApplication1.Models
{
    public class Book:BaseEntity
    {
        public string Title { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
