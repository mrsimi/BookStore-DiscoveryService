using System.Net;
using BookStore_DiscoveryService.DataContext;
using BookStore_DiscoveryService.DTO.Requests;
using BookStore_DiscoveryService.Services.Implementation;
using BookStore_DiscoveryService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BookStoreDbContext>(options => {
    options.UseSqlite(connectionString: connectionString);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookStoreService, BookStoreService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("api/discvoery/books", async([FromQuery]string bookTitle, [FromQuery]string genre, [FromQuery]string authorName, IBookStoreService bookStoreService) => {
    var result = await bookStoreService.GetAll(genre, authorName, bookTitle);
    switch (result.StatusCode)
    {
        case (int)HttpStatusCode.OK:
            return Results.Ok(result);
        case (int)HttpStatusCode.BadRequest:
            return Results.BadRequest(result);
        case (int)HttpStatusCode.NotFound:
            return Results.NotFound(result);
        default:
            return Results.Problem("An error occured.", statusCode: result.StatusCode);
    }
});

app.MapGet("api/discovery/books/{bookId}", async(int bookId, IBookStoreService bookStoreService) => {
    var result = await bookStoreService.Get(bookId);
    switch (result.StatusCode)
    {
        case (int)HttpStatusCode.OK:
            return Results.Ok(result);
        case (int)HttpStatusCode.BadRequest:
            return Results.BadRequest(result);
        case (int)HttpStatusCode.NotFound:
            return Results.NotFound(result);
        default:
            return Results.Problem("An error occured.", statusCode: result.StatusCode);
    }
});

app.MapPost("api/discovery/books", async (BookRequest request, IBookStoreService bookStoreService) =>
{
    var result = await bookStoreService.Add(request);

    switch (result.StatusCode)
    {
        case (int)HttpStatusCode.OK:
            return Results.Ok(result);
        case (int)HttpStatusCode.Conflict:
            return Results.Conflict(result);
        case (int)HttpStatusCode.NotFound:
            return Results.NotFound(result);
        default:
            return Results.Problem("An error occured.", statusCode: result.StatusCode);
    }
});
app.Run();
