using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_DiscoveryService.Models
{
    public class Book
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("Genre")]
        public int GenreId {get; set;}
        [ForeignKey("Author")]
        public int AuthorId {get; set;}
        public Genre Genre { get; set; }
        
        public Author Author {get; set;}
    }
}