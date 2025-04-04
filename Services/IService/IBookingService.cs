using Repositories.Entities;
using Repositories.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IBookingService
    {
        void SaveBooking(List<BookingDetailModel> bookingModels, int userId);
        PaginatedBookingResponse GetBookings(int? status, int? slotId, string roomName, int pageNumber = 1, int pageSize = 10);
        BookingModel GetBookingById(int bookingId);
        bool UpdateBooking(BookingModel updatedBooking);
        Task<bool> ApproveBooking(int bookingDetailId, bool status);

        bool DeleteBooking(int bookingId);
        PaginatedBookingResponse GetBookingForHeadDepartments(int? status, int pageNumber = 1, int pageSize = 10);
        BookingModel GetBookingByBookingDetailId(int bookingDetailId, int bookingId);
        PaginatedBookingResponse GetBookingForManagers(int? status, int pageNumber = 1, int pageSize = 10);
    }
}
