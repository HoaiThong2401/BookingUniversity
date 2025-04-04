using Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModel
{
    public class BookingDetailViewModel
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int SlotId { get; set; }
        public int RoomId { get; set; }

        public TimeOnly SlotStartTime { get; set; }
        public TimeOnly SlotEndTime { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Reason { get; set; }
    }
}
