using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.IRepository;
using Repositories.ViewModel;
using Services.IService;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;



namespace Services.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingService(IUnitOfWork unitOfWork, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public void SaveBooking(List<BookingDetailModel> bookingModels, int userId)
        {
            if (bookingModels == null || !bookingModels.Any())
                throw new ArgumentException("No booking models provided.");

            var booking = new Booking
            {
                UserId = userId,
                Status = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();

            foreach (var bookingModel in bookingModels)
            {
                var bookingDetail = new BookingDetail
                {
                    BookingId = booking.Id,
                    BookingDate = DateOnly.FromDateTime(bookingModel.Date ?? DateTime.UtcNow),
                    SlotId = bookingModel.SlotId,
                    RoomId = bookingModel.RoomId,
                    Status = 0,
                    Reason = bookingModel.Reason,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _unitOfWork.BookingDetailRepository.Insert(bookingDetail);
            }

            _unitOfWork.Save();
        }

        public PaginatedBookingResponse GetBookings(int? status, int? slotId, string roomName, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.BookingRepository.Entities
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Slot)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            if (slotId.HasValue)
            {
                query = query.Where(b => b.BookingDetails.Any(bd => bd.SlotId == slotId.Value));
            }

            if (!string.IsNullOrEmpty(roomName))
            {
                query = query.Where(b => b.BookingDetails.Any(bd => bd.Room.Name.Contains(roomName)));
            }

            var totalItems = query.Count();
            var bookings = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingModel
                {
                    BookingId = b.Id,
                    UserId = b.UserId,
                    Status = b.Status,
                    BookingDetails = b.BookingDetails.Select(bd => new BookingDetailViewModel
                    {
                        Id = bd.Id,
                        BookingDate = bd.BookingDate.ToDateTime(new TimeOnly(0, 0)),
                        SlotId = bd.SlotId,
                        SlotStartTime = bd.Slot.StartTime,
                        SlotEndTime = bd.Slot.EndTime,
                        RoomName = bd.Room.Name,
                        Reason = bd.Reason,
                        Status = bd.Status,
                    }).ToList()
                }).ToList();

            return new PaginatedBookingResponse
            {
                TotalItems = totalItems,
                Bookings = bookings,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public BookingModel GetBookingById(int bookingId)
        {
            var booking = _unitOfWork.BookingRepository.Entities
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Slot)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .Include(b => b.User)
                .FirstOrDefault(b => b.Id == bookingId && b.DeletedAt == null);

            if (booking == null)
                return null;

            var bookingModel = new BookingModel
            {
                BookingId = booking.Id,
                UserId = booking.UserId,
                Status = booking.Status,
                DepartmentId = booking.User != null ? booking.User.DepartmentId : 0,
                CampusId = booking.User != null ? booking.User.CampusId : 0,
                BookingDetails = booking.BookingDetails.Select(bd =>
                {
                    var bookingDetail = new BookingDetailViewModel
                    {
                        Id = bd.Id,
                        BookingDate = bd.BookingDate.ToDateTime(new TimeOnly(0, 0)),
                        SlotId = bd.SlotId,
                        SlotStartTime = bd.Slot?.StartTime ?? default(TimeOnly),
                        SlotEndTime = bd.Slot?.EndTime ?? default(TimeOnly),
                        RoomName = bd.Room?.Name ?? "Unknown",
                        Reason = bd.Reason,
                        Status = bd.Status
                    };

                    return bookingDetail;
                }).ToList()
            };

            Console.WriteLine($"Check: {bookingModel}");
            return bookingModel;


        }

        public bool UpdateBooking(BookingModel updatedBooking)
        {
            var booking = _unitOfWork.BookingRepository.Entities
                .Include(b => b.BookingDetails)
                .FirstOrDefault(b => b.Id == updatedBooking.BookingId && b.DeletedAt == null);

            if (booking == null || booking.Status != 0)
            {
                return false;
            }
               

            booking.UpdatedAt = DateTime.UtcNow;

            foreach (var detail in updatedBooking.BookingDetails)
            {
                var existingDetail = booking.BookingDetails.FirstOrDefault(bd => bd.Id == detail.Id);
                if (existingDetail != null)
                {
                    existingDetail.BookingDate = DateOnly.FromDateTime(detail.BookingDate);
                    existingDetail.SlotId = detail.SlotId;
                    existingDetail.RoomId = detail.RoomId;
                    existingDetail.Reason = detail.Reason;
                    existingDetail.UpdatedAt = DateTime.UtcNow;
                }
            }

            _unitOfWork.Save();
            return true;
        }

        public bool DeleteBooking(int bookingId)
        {
            var booking = _unitOfWork.BookingRepository.Entities.FirstOrDefault(b => b.Id == bookingId && b.DeletedAt == null);
            if (booking == null || booking.Status != 0)
            {
                return false;
            }
            booking.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Save();
            return true;
        }

        public PaginatedBookingResponse GetBookingForHeadDepartments(int? status, int pageNumber = 1, int pageSize = 10)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdStr, out int userId))
            {
                throw new Exception("Invalid user ID");
            }

            var currentUser = _unitOfWork.UserRepository.Entities.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
            {
                throw new Exception("User not found");
            }

            var departmentId = currentUser.DepartmentId;

            var userIdsInDepartment = _unitOfWork.UserRepository.Entities
                .Where(u => u.DepartmentId == departmentId)
                .Select(u => u.Id)
                .ToList();
            var bookingIdsWithStatus = _unitOfWork.BookingDetailRepository.Entities
                .Where(bd => status.HasValue && bd.Status == status.Value)
                .Select(bd => bd.BookingId)
                .ToList();
            var query = _unitOfWork.BookingRepository.Entities
                .Include(b => b.User)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Slot)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Room)
               .Where(b => userIdsInDepartment.Contains(b.UserId) && bookingIdsWithStatus.Contains(b.Id))
                .AsQueryable();

            var totalItems = query.Count();
            var bookings = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingModel
                {
                    BookingId = b.Id,
                    UserId = b.UserId,
                    Status = b.Status,
                    UserName = b.User.FullName,
                    BookingDetails = b.BookingDetails.Select(bd => new BookingDetailViewModel
                    {
                        Id = bd.Id,
                        BookingDate = bd.BookingDate.ToDateTime(new TimeOnly(0, 0)),
                        SlotId = bd.SlotId,
                        SlotStartTime = bd.Slot.StartTime,
                        SlotEndTime = bd.Slot.EndTime,
                        RoomName = bd.Room.Name,
                        Reason = bd.Reason,
                        Status = bd.Status,
                    }).ToList()
                }).ToList();

            return new PaginatedBookingResponse
            {
                TotalItems = totalItems,
                Bookings = bookings,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public BookingModel GetBookingByBookingDetailId(int bookingDetailId, int bookingId)
        {
            var bookingDetail = _unitOfWork.BookingDetailRepository.Entities
                .FirstOrDefault(bd => bd.Id == bookingDetailId && bd.BookingId == bookingId);

            if (bookingDetail == null)
            {
                return null;
            }

            return GetBookingById(bookingDetail.BookingId);
        }

        public PaginatedBookingResponse GetBookingForManagers(int? status, int pageNumber = 1, int pageSize = 10)
        {
            var bookingIdsWithApproval = _unitOfWork.ApprovalHistoryRepository.Entities
                .Where(ah => ah.ApprovalByHeadDepartment == true)
                .Select(ah => ah.BookingId)
                .Distinct()
                .ToList();
            var query = _unitOfWork.BookingRepository.Entities
               .Include(b => b.User)
               .Include(b => b.BookingDetails)
                   .ThenInclude(bd => bd.Slot)
               .Include(b => b.BookingDetails)
                   .ThenInclude(bd => bd.Room)
               .Where(b => bookingIdsWithApproval.Contains(b.Id));

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }
            var totalItems = query.Count();

            var bookings = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingModel
                {
                    BookingId = b.Id,
                    UserId = b.UserId,
                    Status = b.Status,
                    UserName = b.User.FullName,
                    BookingDetails = b.BookingDetails
                        .Select(bd => new BookingDetailViewModel
                        {
                            Id = bd.Id,
                            BookingDate = bd.BookingDate.ToDateTime(new TimeOnly(0, 0)),
                            SlotId = bd.SlotId,
                            SlotStartTime = bd.Slot.StartTime,
                            SlotEndTime = bd.Slot.EndTime,
                            RoomName = bd.Room.Name,
                            Reason = bd.Reason,
                            Status = bd.Status,
                        }).ToList()
                }).ToList();

            return new PaginatedBookingResponse
            {
                TotalItems = totalItems,
                Bookings = bookings,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


    }
}
