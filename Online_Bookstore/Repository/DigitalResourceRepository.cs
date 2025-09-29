using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class DigitalResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public DigitalResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DigitalResource>> GetAllAsync()
        {
            return await _context.DigitalResources.Include(r => r.Book).ToListAsync();
        }

        public async Task<DigitalResource> GetByIdAsync(int id)
        {
            return await _context.DigitalResources.Include(r => r.Book).FirstOrDefaultAsync(r => r.ResourceId == id);
        }

        public async Task SaveAsync(DigitalResource resource)
        {
            if (resource.ResourceId == 0)
            {
                _context.DigitalResources.Add(resource);
            }
            else
            {
                _context.Entry(resource).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var resource = await _context.DigitalResources.FindAsync(id);
            if (resource != null)
            {
                _context.DigitalResources.Remove(resource);
                await _context.SaveChangesAsync();
            }
        }
    }
}