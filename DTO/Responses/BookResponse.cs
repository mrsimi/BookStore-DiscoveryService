using BookStore_DiscoveryService.DTO.Requests;

namespace BookStore_DiscoveryService.DTO.Responses
{
    public class BookResponse : BookRequest
    {
        public int Id { get; set; }
    }
}