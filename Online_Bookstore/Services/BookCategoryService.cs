using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class BookCategoryService
    {
        private readonly BookCategoryRepository _bookCategoryRepository;

        public BookCategoryService(BookCategoryRepository bookCategoryRepository)
        {
            _bookCategoryRepository = bookCategoryRepository;
        }

        public async Task<List<BookCategory>> GetAllCategoriesAsync()
        {
            return await _bookCategoryRepository.GetAllAsync();
        }

        // Synchronous wrappers for legacy controller calls
        public List<BookCategory> GetAllCategories()
        {
            return GetAllCategoriesAsync().GetAwaiter().GetResult();
        }

        public async Task<BookCategory> GetCategoryByIdAsync(int id)
        {
            return await _bookCategoryRepository.GetByIdAsync(id);
        }

        public BookCategory GetCategoryById(int id)
        {
            return GetCategoryByIdAsync(id).GetAwaiter().GetResult();
        }

        public async Task SaveCategoryAsync(BookCategory category)
        {
            await _bookCategoryRepository.SaveAsync(category);
        }

        public void SaveCategory(BookCategory category)
        {
            SaveCategoryAsync(category).GetAwaiter().GetResult();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _bookCategoryRepository.DeleteAsync(id);
        }

        public void DeleteCategory(int id)
        {
            DeleteCategoryAsync(id).GetAwaiter().GetResult();
        }

        public async Task<BookCategory> FindByCategoryNameAsync(string name)
        {
            return await _bookCategoryRepository.FindByCategoryNameAsync(name);
        }
    }
}