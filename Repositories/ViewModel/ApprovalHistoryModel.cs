using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModel
{
    public class ApprovalHistoryModel
    {
        public int Id { get; set; }
        public int CampusId { get; set; }
        public int DepartmentId { get; set; }
        public int UserBookingId { get; set; }
        public int BookingId { get; set; }
        public int BookingDetailId { get; set; }
        public bool ApprovalByManager { get; set; }
        public string? ReasonByManager { get; set; }
        public bool ApprovalByHeadDepartment { get; set; }
        public string? ReasonByHeadApproval { get; set; }
    }
}
