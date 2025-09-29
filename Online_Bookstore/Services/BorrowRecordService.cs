using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class BorrowRecordService
    {
        private readonly BorrowRecordRepository _borrowRecordRepository;
        private readonly UserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowRecordService(
            BorrowRecordRepository borrowRecordRepository,
            UserRepository userRepository,
            IBookRepository bookRepository)
        {
            _borrowRecordRepository = borrowRecordRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            return await _borrowRecordRepository.GetAllAsync();
        }

        public async Task<BorrowRecord> GetBorrowRecordByIdAsync(int id)
        {
            var record = await _borrowRecordRepository.GetByIdAsync(id);
            if (record == null)
                throw new Exception($"Không tìm thấy BorrowRecord với ID: {id}");
            return record;
        }

        public async Task SaveBorrowRecordAsync(BorrowRecord record)
        {
            if (record.UserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(record.UserId.Value);
                if (user == null)
                    throw new Exception($"Không tìm thấy người dùng với ID: {record.UserId}");
                record.User = user;
            }

            if (record.BookId.HasValue)
            {
                var book = await _bookRepository.GetByIdAsync(record.BookId.Value);
                if (book == null)
                    throw new Exception($"Không tìm thấy sách với ID: {record.BookId}");
                record.Book = book;
            }

            await _borrowRecordRepository.SaveAsync(record);
        }

        public async Task DeleteBorrowRecordAsync(int id)
        {
            await _borrowRecordRepository.DeleteAsync(id);
        }

        public async Task<List<(Book, int)>> GetTopBorrowedBooksAsync(int limit)
        {
            var records = await _borrowRecordRepository.GetAllAsync();
            var topBooks = records
                .Where(r => r.Book != null)
                .GroupBy(r => r.Book)
                .Select(g => new { Book = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(limit)
                .ToList();
            return topBooks.Select(x => (x.Book, x.Count)).ToList();
        }

        public async Task<Dictionary<string, long>> GetBorrowRecordsByStatusAsync()
        {
            var records = await GetAllBorrowRecordsAsync();
            return records.GroupBy(r => string.IsNullOrEmpty(r.Status) ? "Không xác định" : r.Status)
                          .ToDictionary(g => g.Key, g => (long)g.Count());
        }

        public async Task<List<(User, int)>> GetTopActiveUsersAsync(int limit)
        {
            var records = await _borrowRecordRepository.GetAllAsync();
            var topUsers = records
                .Where(r => r.User != null)
                .GroupBy(r => r.User)
                .Select(g => new { User = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(limit)
                .ToList();
            return topUsers.Select(x => (x.User, x.Count)).ToList();
        }

        public async Task<Dictionary<string, long>> GetBorrowRecordsByMonthAsync()
        {
            var records = await GetAllBorrowRecordsAsync();
            return records
                .Where(r => r.BorrowDate.HasValue)
                .GroupBy(r => r.BorrowDate.Value.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => (long)g.Count());
        }

        public async Task<List<BorrowRecord>> GetOverdueBooksAsync()
        {
            var now = DateTime.Now;
            var records = await _borrowRecordRepository.GetAllAsync();
            return records
                .Where(r => r.DueDate.HasValue && r.DueDate.Value < now && (string.IsNullOrEmpty(r.Status) || r.Status == "Đang mượn"))
                .ToList();
        }
    }
}