using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Slot> SlotRepository { get; }
        IGenericRepository<Room> RoomRepository { get; }
        IGenericRepository<ApprovalHistory> ApprovalHistoryRepository { get; }
        IGenericRepository<Booking> BookingRepository { get; }
        IGenericRepository<BookingDetail> BookingDetailRepository { get; }
        IGenericRepository<Campus> CampusRepository { get; }
        IGenericRepository<Department> DepartmentRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }

    }
}
