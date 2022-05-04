using BookStore_DiscoveryService.DTO.Requests;
using BookStore_DiscoveryService.DTO.Responses;

namespace BookStore_DiscoveryService.Services.Interfaces
{
    public interface IBookStoreService
    {
        Task<GenericResponse<BookResponse>> Add(BookRequest request);
        Task<GenericResponse<BookResponse>> Get(int bookId);
        Task<GenericResponse<List<BookResponse>>> GetAll(string genreFilter, string authorFilter, string titleFilter);
    }
}