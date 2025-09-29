using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task SaveAsync(Book book);
        Task DeleteAsync(int id);
        Task<Book> FindByIsbnAsync(string isbn);
        Task<List<Book>> FindByTitleContainingAsync(string title);
        Task<List<Book>> FindByAuthorContainingAsync(string author);
        Task<List<Book>> FindByCategoryNameAsync(string categoryName);
    }

    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.Include(b => b.Category).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task SaveAsync(Book book)
        {
            if (book.BookId == 0)
            {
                _context.Books.Add(book);
            }
            else
            {
                _context.Entry(book).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> FindByIsbnAsync(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);
        }

        public async Task<List<Book>> FindByTitleContainingAsync(string title)
        {
            return await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
        }

        public async Task<List<Book>> FindByAuthorContainingAsync(string author)
        {
            return await _context.Books.Where(b => b.Author.Contains(author)).ToListAsync();
        }

        public async Task<List<Book>> FindByCategoryNameAsync(string categoryName)
        {
            return await _context.Books.Where(b => b.Category.CategoryName == categoryName).ToListAsync();
        }
    }
}