using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task SaveBookAsync(Book book)
        {
            await _bookRepository.SaveAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<Book> FindByIsbnAsync(string isbn)
        {
            return await _bookRepository.FindByIsbnAsync(isbn);
        }

        public async Task<List<Book>> SearchByTitleAsync(string title)
        {
            return await _bookRepository.FindByTitleContainingAsync(title);
        }

        public async Task<List<Book>> SearchByAuthorAsync(string author)
        {
            return await _bookRepository.FindByAuthorContainingAsync(author);
        }

        public async Task<List<Book>> SearchByIsbnAsync(string isbn)
        {
            var book = await _bookRepository.FindByIsbnAsync(isbn);
            return book != null ? new List<Book> { book } : new List<Book>();
        }

        public async Task<List<Book>> SearchByCategoryAsync(string categoryName)
        {
            return await _bookRepository.FindByCategoryNameAsync(categoryName);
        }


        // IBookService interface methods
        public async Task<List<Book>> FindBooksByCategoryAsync(int categoryId)
        {
            var books = await GetAllBooksAsync();
            return books.Where(b => b.CategoryId == categoryId).ToList();
        }

        public async Task<List<Book>> SearchBooksAsync(string keyword)
        {
            var books = await GetAllBooksAsync();
            return books.Where(b => 
                b.Title.Contains(keyword) || 
                b.Author.Contains(keyword) || 
                b.Description.Contains(keyword)).ToList();
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(string author)
        {
            return await SearchByAuthorAsync(author);
        }

        public async Task<List<Book>> GetRecentBooksAsync(int limit)
        {
            var books = await GetAllBooksAsync();
            return books.OrderByDescending(b => b.CreatedAt).Take(limit).ToList();
        }

        public async Task<Dictionary<string, int>> GetBooksByCategoryAsync()
        {
            var books = await GetAllBooksAsync();
            return books
                .GroupBy(b => b.Category != null ? b.Category.CategoryName : "Không phân loại")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetBooksByAvailabilityAsync()
        {
            var books = await GetAllBooksAsync();
            return books
                .GroupBy(b => b.AvailableCopies > 0 ? "Có sẵn" : "Hết sách")
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}