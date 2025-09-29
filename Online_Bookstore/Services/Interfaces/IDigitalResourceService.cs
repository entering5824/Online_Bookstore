using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IDigitalResourceService
    {
        Task<List<DigitalResource>> GetAllDigitalResourcesAsync();
        Task<DigitalResource> GetDigitalResourceByIdAsync(int id);
        Task SaveDigitalResourceAsync(DigitalResource resource);
        Task DeleteDigitalResourceAsync(int id);
    }
}