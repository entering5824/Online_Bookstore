using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services.Interfaces;
using System;
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
            try
            {
                System.Diagnostics.Debug.WriteLine("🔍 BookService.GetAllBooksAsync: Starting...");
                var result = await _bookRepository.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"✅ BookService.GetAllBooksAsync: Successfully loaded {result.Count} books");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Lỗi trong GetAllBooksAsync: " + ex.Message);
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
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
            // Fallback to existing method - will be optimized later
            return await SearchByCategoryAsync(categoryId.ToString());
        }

        public async Task<List<Book>> SearchBooksAsync(string keyword)
        {
            // Fallback to existing method - will be optimized later
            var titleResults = await SearchByTitleAsync(keyword);
            var authorResults = await SearchByAuthorAsync(keyword);
            
            // Combine and remove duplicates
            var allResults = titleResults.Concat(authorResults)
                .GroupBy(b => b.BookId)
                .Select(g => g.First())
                .ToList();
                
            return allResults;
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(string author)
        {
            return await SearchByAuthorAsync(author);
        }

        public async Task<List<Book>> GetRecentBooksAsync(int limit)
        {
            // Temporarily use existing method - will be optimized later
            var books = await GetAllBooksAsync();
            return books.OrderByDescending(b => b.CreatedDate).Take(limit).ToList();
        }

        public async Task<Dictionary<string, int>> GetBooksByCategoryAsync()
        {
            // Temporarily use existing method - will be optimized later
            var books = await GetAllBooksAsync();
            return books
                .GroupBy(b => b.Category != null ? b.Category.CategoryName : "Không phân loại")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetBooksByAvailabilityAsync()
        {
            // Temporarily use existing method - will be optimized later
            var books = await GetAllBooksAsync();
            return books
                .GroupBy(b => b.AvailableCopies > 0 ? "Có sẵn" : "Hết sách")
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}