using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IBookCategoryService
    {
        Task<List<BookCategory>> GetAllCategoriesAsync();
        Task<BookCategory> GetCategoryByIdAsync(int id);
        Task SaveCategoryAsync(BookCategory category);
        Task DeleteCategoryAsync(int id);
        Task<BookCategory> FindByCategoryNameAsync(string name);
    }
}