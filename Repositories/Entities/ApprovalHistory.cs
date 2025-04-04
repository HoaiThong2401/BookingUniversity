using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class ApprovalHistory
{
    public int Id { get; set; }

    public int CampusId { get; set; }

    public int DepartmentId { get; set; }

    public int UserBookingId { get; set; }

    public int BookingId { get; set; }

    public int BookingDetailId { get; set; }

    public int HeadDepartmentId { get; set; }

    public bool ApprovalByHeadDepartment { get; set; }

    public string? ReasonByHeadApproval { get; set; }

    public int ManagerId { get; set; }

    public bool ApprovalByManager { get; set; }

    public string? ReasonByManager { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual BookingDetail BookingDetail { get; set; } = null!;

    public virtual Campus Campus { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual User HeadDepartment { get; set; } = null!;

    public virtual User Manager { get; set; } = null!;

    public virtual User UserBooking { get; set; } = null!;
}
