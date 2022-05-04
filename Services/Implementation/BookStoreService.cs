using System.Net;
using BookStore_DiscoveryService.DataContext;
using BookStore_DiscoveryService.DTO.Requests;
using BookStore_DiscoveryService.DTO.Responses;
using BookStore_DiscoveryService.Models;
using BookStore_DiscoveryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore_DiscoveryService.Services.Implementation
{
    public class BookStoreService : IBookStoreService
    {
        private readonly BookStoreDbContext _context;
        public BookStoreService(BookStoreDbContext context)
        {
            _context = context;   
        }


        public async Task<GenericResponse<BookResponse>> Add(BookRequest request)
        {
            try
            {
                var oldBook = await _context.Books.Where(m => m.Title.ToLower().Contains(request.Title.ToLower()) && m.Year == request.Year).FirstOrDefaultAsync();
                if(oldBook != null)
                {
                    return new GenericResponse<BookResponse>
                    {
                        Data = null, 
                        StatusCode = (int)HttpStatusCode.Conflict, 
                        ResponseMessage = "This Book already exists"
                    };
                }

                string[] authorNames = request.AuthorFullName.ToLower().Split(" ");

                var author = await _context.Authors.FirstOrDefaultAsync(m => authorNames.Contains(m.FirstName.ToLower()) 
                    && authorNames.Contains(m.LastName.ToLower()));

                
                
                if(author == null)
                {
                    var newAuthor = new Models.Author{FirstName = authorNames[0], LastName = authorNames[1]};
                    author = newAuthor;
                }
                

                var genre = await _context.Genres.FirstOrDefaultAsync(m => m.Name.ToLower().Contains(request.GenreName.ToLower()));
                if (genre == null)
                {
                    var newGenre = new Genre{Name = request.GenreName};
                    genre = newGenre;
                }

                var newBook = new Book
                {
                    Title = request.Title, 
                    Year = request.Year, 
                    Price = request.Price, 
                    Genre = genre, 
                    Author = author
                };


                await _context.AddAsync(newBook);

                await _context.SaveChangesAsync();


                return new GenericResponse<BookResponse>
                {
                    Data = new BookResponse
                    {
                        Id = newBook.Id, 
                        Title = newBook.Title, 
                        Price = newBook.Price,
                        Year = newBook.Year, 
                        GenreName = genre.Name, 
                        AuthorFullName = author.FullName()
                    },
                    StatusCode = (int)HttpStatusCode.OK, 
                    ResponseMessage = "Book added successfully"
                };
                
            }

            catch (System.Exception)
            {
                return new GenericResponse<BookResponse>
                {
                    Data = null, 
                    StatusCode = (int)HttpStatusCode.InternalServerError, 
                    ResponseMessage = "Internal Server Error"
                };
            }
        }

        public async Task<GenericResponse<BookResponse>> Get(int bookId)
        {
            try
            {
                var book = await _context.Books.Include(m => m.Author).Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == bookId);
                if(book != null)
                {
                    return new GenericResponse<BookResponse>
                    {
                        Data = new BookResponse
                        {
                            Id = book.Id, 
                            Title = book.Title, 
                            Year = book.Year, 
                            Price = book.Price, 
                            AuthorFullName = book.Author.FullName(), 
                            GenreName = book.Genre.Name
                        },
                        StatusCode = (int)HttpStatusCode.OK, 
                        ResponseMessage = "Book retrieved successfully"
                    };
                }
                else
                {
                    return new GenericResponse<BookResponse>
                    {
                        Data = null,
                        StatusCode = (int)HttpStatusCode.NotFound, 
                        ResponseMessage = $"Book with Id: {bookId} not found"
                    };
                }
            }
            catch (System.Exception)
            {
                return new GenericResponse<BookResponse>
                {
                    Data = null, 
                    StatusCode = (int)HttpStatusCode.InternalServerError, 
                    ResponseMessage = "Internal Server Error"
                };
            }
        }

        public async Task<GenericResponse<List<BookResponse>>> GetAll(string genreFilter, string authorFilter, string titleFilter)
        {
            try
            {
                var books = await _context.Books.Include(m => m.Author)
                    .Include(m => m.Genre)
                    .Select(m => 
                    new BookResponse
                    {
                        Id = m.Id, 
                        Year = m.Year,
                        Title = m.Title,  
                        Price = m.Price, 
                        GenreName = m.Genre.Name, 
                        AuthorFullName = m.Author.FullName()
                    }).ToListAsync();

                StringComparison comp = StringComparison.OrdinalIgnoreCase;

                if(!string.IsNullOrEmpty(titleFilter))
                {
                    books = books.Where(m => m.Title.Contains(titleFilter, comp)).ToList();
                }

                if(!string.IsNullOrEmpty(genreFilter))
                {
                    books = books.Where(m => m.GenreName.Contains(genreFilter, comp)).ToList();
                }
                
                if(!string.IsNullOrEmpty(authorFilter))
                {
                    books = books.Where(m => m.AuthorFullName.Contains(authorFilter, comp)).ToList();
                }
               

               return new GenericResponse<List<BookResponse>>
               {
                   Data = books, 
                   StatusCode = (int)HttpStatusCode.OK, 
                   ResponseMessage = "Books retrieved successfully"
               };
            }
            catch (System.Exception)
            {
                return new GenericResponse<List<BookResponse>>
                {
                    Data = null, 
                    StatusCode = (int)HttpStatusCode.InternalServerError, 
                    ResponseMessage = "Internal Server Error"
                };
            }
        }
    }
}