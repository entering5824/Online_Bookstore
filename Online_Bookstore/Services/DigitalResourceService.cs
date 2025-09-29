using Online_Bookstore.Models;
using Online_Bookstore.Repository;

using Online_Bookstore.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class DigitalResourceService : IDigitalResourceService
    {
        private readonly DigitalResourceRepository _digitalResourceRepository;
        private readonly IBookRepository _bookRepository;

        public DigitalResourceService(DigitalResourceRepository digitalResourceRepository, IBookRepository bookRepository)
        {
            _digitalResourceRepository = digitalResourceRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<DigitalResource>> GetAllDigitalResourcesAsync()
        {
            return await _digitalResourceRepository.GetAllAsync();
        }

        public async Task<DigitalResource> GetDigitalResourceByIdAsync(int id)
        {
            var resource = await _digitalResourceRepository.GetByIdAsync(id);
            if (resource == null)
                throw new Exception($"Không tìm thấy tài nguyên số với ID: {id}");
            return resource;
        }

        public async Task SaveDigitalResourceAsync(DigitalResource resource)
        {
            if (resource.BookId.HasValue)
            {
                var book = await _bookRepository.GetByIdAsync(resource.BookId.Value);
                if (book == null)
                    throw new Exception($"Không tìm thấy sách với ID: {resource.BookId}");
                resource.Book = book;
            }

            await _digitalResourceRepository.SaveAsync(resource);
        }

        public async Task DeleteDigitalResourceAsync(int id)
        {
            await _digitalResourceRepository.DeleteAsync(id);
        }
    }
}