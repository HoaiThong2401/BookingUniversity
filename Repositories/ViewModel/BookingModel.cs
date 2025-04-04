using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModel
{
    public class BookingModel
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; }
        public int DepartmentId { get; set; }
        public int CampusId { get; set; }
        public List<BookingDetailViewModel> BookingDetails { get; set; } = new List<BookingDetailViewModel>();
    }
}
