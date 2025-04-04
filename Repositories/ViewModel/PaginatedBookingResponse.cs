using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModel
{
    public class PaginatedBookingResponse
    {
        public int TotalItems { get; set; }
        public List<BookingModel> Bookings { get; set; } = new List<BookingModel>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
