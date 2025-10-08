using Online_Bookstore.Models;
using System;
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
            try
            {
                System.Diagnostics.Debug.WriteLine("🔍 BorrowRecordRepository.GetAllAsync: Starting...");
                var result = await _context.BorrowRecords.Include(r => r.User).Include(r => r.Book).ToListAsync();
                System.Diagnostics.Debug.WriteLine($"✅ BorrowRecordRepository.GetAllAsync: Successfully loaded {result.Count} borrow records");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Lỗi trong BorrowRecordRepository.GetAllAsync: " + ex.Message);
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
        }

        public async Task<BorrowRecord> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords.Include(r => r.User).Include(r => r.Book).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task SaveAsync(BorrowRecord record)
        {
            if (record.Id == 0)
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