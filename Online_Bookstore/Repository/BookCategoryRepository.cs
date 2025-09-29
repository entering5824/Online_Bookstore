using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class BookCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public BookCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookCategory>> GetAllAsync()
        {
            return await _context.BookCategories.ToListAsync();
        }

        public async Task<BookCategory> GetByIdAsync(int id)
        {
            return await _context.BookCategories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task SaveAsync(BookCategory category)
        {
            if (category.CategoryId == 0)
            {
                _context.BookCategories.Add(category);
            }
            else
            {
                _context.Entry(category).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category != null)
            {
                _context.BookCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BookCategory> FindByCategoryNameAsync(string name)
        {
            return await _context.BookCategories.FirstOrDefaultAsync(c => c.CategoryName == name);
        }
    }
}