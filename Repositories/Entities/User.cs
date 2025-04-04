using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public int CampusId { get; set; }

    public int RoleId { get; set; }

    public int DepartmentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<ApprovalHistory> ApprovalHistoryHeadDepartments { get; set; } = new List<ApprovalHistory>();

    public virtual ICollection<ApprovalHistory> ApprovalHistoryManagers { get; set; } = new List<ApprovalHistory>();

    public virtual ICollection<ApprovalHistory> ApprovalHistoryUserBookings { get; set; } = new List<ApprovalHistory>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Campus Campus { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
