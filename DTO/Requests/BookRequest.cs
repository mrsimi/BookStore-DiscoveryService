namespace BookStore_DiscoveryService.DTO.Requests
{
    public class BookRequest
    {
        public string Title { get; set; }
        public int  Year { get; set; }
        public decimal Price {get; set;}
        public string GenreName {get; set;}
        public string AuthorFullName {get; set;}
    }
}