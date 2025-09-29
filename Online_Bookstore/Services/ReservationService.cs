using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly UserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public ReservationService(
            ReservationRepository reservationRepository,
            UserRepository userRepository,
            IBookRepository bookRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _reservationRepository.GetAllAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            var res = await _reservationRepository.GetByIdAsync(id);
            if (res == null)
                throw new Exception($"Không tìm thấy reservation với ID: {id}");
            return res;
        }

        public async Task SaveReservationAsync(Reservation reservation)
        {
            if (reservation.UserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(reservation.UserId.Value);
                if (user == null)
                    throw new Exception($"Không tìm thấy người dùng với ID: {reservation.UserId}");
                reservation.User = user;
            }

            if (reservation.BookId.HasValue)
            {
                var book = await _bookRepository.GetByIdAsync(reservation.BookId.Value);
                if (book == null)
                    throw new Exception($"Không tìm thấy sách với ID: {reservation.BookId}");
                reservation.Book = book;
            }

            await _reservationRepository.SaveAsync(reservation);
        }

        public async Task DeleteReservationAsync(int id)
        {
            await _reservationRepository.DeleteAsync(id);
        }

        public async Task<List<(Book, int)>> GetTopReservedBooksAsync(int limit)
        {
            var all = await _reservationRepository.GetTopReservedBooksAsync();
            return all.Take(limit).ToList();
        }

        public async Task<Dictionary<string, long>> GetReservationsByStatusAsync()
        {
            var reservations = await GetAllReservationsAsync();
            return reservations
                .GroupBy(r => string.IsNullOrEmpty(r.Status) ? "Không xác định" : r.Status)
                .ToDictionary(g => g.Key, g => (long)g.Count());
        }
    }
}