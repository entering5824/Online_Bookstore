using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IBorrowRecordService
    {
        Task<List<BorrowRecord>> GetAllBorrowRecordsAsync();
        Task<BorrowRecord> GetBorrowRecordByIdAsync(int id);
        Task SaveBorrowRecordAsync(BorrowRecord record);
        Task DeleteBorrowRecordAsync(int id);
        Task<List<(Book, int)>> GetTopBorrowedBooksAsync(int limit);
        Task<Dictionary<string, long>> GetBorrowRecordsByStatusAsync();
        Task<List<(User, int)>> GetTopActiveUsersAsync(int limit);
        Task<Dictionary<string, long>> GetBorrowRecordsByMonthAsync();
        Task<List<BorrowRecord>> GetOverdueBooksAsync();
    }
}