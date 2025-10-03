using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();              // Lấy tất cả sách
        Task<Book> GetBookByIdAsync(int id);              // Tìm sách theo ID
        Task SaveBookAsync(Book book);                    // Thêm hoặc cập nhật sách
        Task DeleteBookAsync(int id);                     // Xóa sách theo ID

        Task<List<Book>> FindBooksByCategoryAsync(int categoryId);  // Tìm sách theo thể loại
        Task<List<Book>> SearchBooksAsync(string keyword);          // Tìm kiếm theo từ khóa (tên, tác giả, mô tả)
        Task<List<Book>> GetBooksByAuthorAsync(string author);      // Lấy sách theo tác giả
        Task<List<Book>> GetRecentBooksAsync(int limit);            // Lấy sách mới nhất (theo ngày phát hành/thêm)
        Task<Dictionary<string, int>> GetBooksByCategoryAsync();
        Task<Dictionary<string, int>> GetBooksByAvailabilityAsync();

    }
}
