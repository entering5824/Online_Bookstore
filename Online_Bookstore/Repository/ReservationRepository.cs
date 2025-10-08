using Online_Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class ReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🔍 ReservationRepository.GetAllAsync: Starting...");
                var result = await _context.Reservations.Include(r => r.User).Include(r => r.Book).ToListAsync();
                System.Diagnostics.Debug.WriteLine($"✅ ReservationRepository.GetAllAsync: Successfully loaded {result.Count} reservations");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Lỗi trong ReservationRepository.GetAllAsync: " + ex.Message);
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations.Include(r => r.User).Include(r => r.Book).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task SaveAsync(Reservation reservation)
        {
            if (reservation.Id == 0)
            {
                _context.Reservations.Add(reservation);
            }
            else
            {
                _context.Entry(reservation).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<(Book, int)>> GetTopReservedBooksAsync()
        {
            var grouped = await _context.Reservations
                .Where(r => r.Book != null)
                .GroupBy(r => r.Book)
                .Select(g => new { Book = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
            return grouped.Select(x => (x.Book, x.Count)).ToList();
        }
    }
}