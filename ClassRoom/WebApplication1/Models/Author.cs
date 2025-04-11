namespace WebApplication1.Models
{
    public class Author:BaseEntity
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
