using Repositories.Entities;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly BookingManagementContext _dbContext;
        private IGenericRepository<User>? _userRepository;
        private IGenericRepository<Slot>? _slotRepository;
        private IGenericRepository<Room>? _roomRepository;
        private IGenericRepository<ApprovalHistory>? _approvalHistoryRepository;
        private IGenericRepository<Booking>? _bookingRepository;
        private IGenericRepository<BookingDetail>? _bookingDetailRepository;
        private IGenericRepository<Campus>? _campusRepository;
        private IGenericRepository<Department>? _departmentRepository;
        private IGenericRepository<Role>? _roleRepository;

        public UnitOfWork(BookingManagementContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<User> UserRepository
             => _userRepository ??= new GenericRepository<User>(_dbContext);
        public IGenericRepository<Slot> SlotRepository
            => _slotRepository ??= new GenericRepository<Slot>(_dbContext);
        public IGenericRepository<Room> RoomRepository
            => _roomRepository ??= new GenericRepository<Room>(_dbContext);
        public IGenericRepository<ApprovalHistory> ApprovalHistoryRepository
            => _approvalHistoryRepository ??= new GenericRepository<ApprovalHistory>(_dbContext);
        public IGenericRepository<Booking> BookingRepository
           => _bookingRepository ??= new GenericRepository<Booking>(_dbContext);
        public IGenericRepository<BookingDetail> BookingDetailRepository
          => _bookingDetailRepository ??= new GenericRepository<BookingDetail>(_dbContext);
        public IGenericRepository<Campus> CampusRepository
            => _campusRepository ??= new GenericRepository<Campus>(_dbContext);
        public IGenericRepository<Department> DepartmentRepository
            => _departmentRepository ??= new GenericRepository<Department>(_dbContext);
        public IGenericRepository<Role> RoleRepository
            => _roleRepository ??= new GenericRepository<Role>(_dbContext);

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("DbContext is being disposed.");
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
