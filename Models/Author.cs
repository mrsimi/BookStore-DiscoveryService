namespace BookStore_DiscoveryService.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public virtual ICollection<Book> Books {get; set;}

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}