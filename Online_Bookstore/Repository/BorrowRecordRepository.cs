using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class BorrowRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowRecord>> GetAllAsync()
        {
            return await _context.BorrowRecords.Include(r => r.User).Include(r => r.Book).ToListAsync();
        }

        public async Task<BorrowRecord> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords.Include(r => r.User).Include(r => r.Book).FirstOrDefaultAsync(r => r.RecordId == id);
        }

        public async Task SaveAsync(BorrowRecord record)
        {
            if (record.RecordId == 0)
            {
                _context.BorrowRecords.Add(record);
            }
            else
            {
                _context.Entry(record).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.BorrowRecords.FindAsync(id);
            if (record != null)
            {
                _context.BorrowRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<(Book, int)>> GetTopBorrowedBooksAsync()
        {
            var grouped = await _context.BorrowRecords
                .Where(r => r.Book != null)
                .GroupBy(r => r.Book)
                .Select(g => new { Book = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
            return grouped.Select(x => (x.Book, x.Count)).ToList();
        }
    }
}