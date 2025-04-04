using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Entities;
using Repositories.ViewModel;
using Services.IService;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace BookingManagementRazorPages.Pages.HeadDepartment
{
    public class ListBookingRequiredModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IApprovalHistoryService _approvalHistoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ListBookingRequiredModel(IBookingService bookingService, IApprovalHistoryService approvalHistoryService, IHttpContextAccessor httpContextAccessor)
        {
            _bookingService = bookingService;
            _approvalHistoryService = approvalHistoryService;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<BookingModel> Bookings { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public IActionResult OnGet(int? status, int pageNumber = 1)
        {
            var paginatedBookings = _bookingService.GetBookingForHeadDepartments(status,pageNumber);

            Bookings = paginatedBookings.Bookings;
            TotalItems = paginatedBookings.TotalItems;
            PageSize = paginatedBookings.PageSize;
            PageNumber = pageNumber;
            return Page();
        }
        public IActionResult OnPostApprove(int bookingDetailId, int bookingId)
        {
            var bookingDetail = _bookingService.GetBookingByBookingDetailId(bookingDetailId, bookingId);

            if (bookingDetail == null)
            {
                return NotFound();
            }
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdStr);
            var approvalHistory = new ApprovalHistoryModel
            {
                BookingId = bookingId,
                BookingDetailId = bookingDetailId,
                ApprovalByHeadDepartment = true,
                ReasonByHeadApproval = "Approved by Head Department",
                UserBookingId = bookingDetail.UserId,
                CampusId = bookingDetail.CampusId,
                DepartmentId = bookingDetail.DepartmentId
            };

            _approvalHistoryService.SaveApprovalHistory(approvalHistory, userId); 

            return RedirectToPage();
        }

        public IActionResult OnPostDeny(int bookingDetailId, int bookingId)
        {
            var bookingDetail = _bookingService.GetBookingByBookingDetailId(bookingDetailId, bookingId);

            if (bookingDetail == null)
            {
                return NotFound();
            }
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value
                ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdStr);
            var approvalHistory = new ApprovalHistoryModel
            {
                BookingId = bookingId,
                BookingDetailId = bookingDetailId,
                ApprovalByHeadDepartment = false,
                ReasonByHeadApproval = "Denied by Head Department",
                UserBookingId = bookingDetail.UserId,
                CampusId = bookingDetail.CampusId,
                DepartmentId = bookingDetail.DepartmentId
            };
            _approvalHistoryService.SaveApprovalHistory(approvalHistory, userId);

            return RedirectToPage();
        }

    }
}
