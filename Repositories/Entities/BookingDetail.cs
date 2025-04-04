using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class BookingDetail
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public DateOnly BookingDate { get; set; }

    public int SlotId { get; set; }

    public int RoomId { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Reason { get; set; }

    public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; } = new List<ApprovalHistory>();

    public virtual Booking Booking { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
