using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task SaveReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(int id);
        Task<List<(Book, int)>> GetTopReservedBooksAsync(int limit);
        Task<Dictionary<string, long>> GetReservationsByStatusAsync();
    }
}