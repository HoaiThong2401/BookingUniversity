using BookingManagementRazorPages.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;
using Services.IService;

namespace BookingManagementRazorPages.Pages.Manager.RoomManagement
{
    public class CreateRoomPageModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly ICampusService _campusService;
        private readonly IHubContext<SignalrSever> _hubContext;

        public CreateRoomPageModel(IRoomService roomService, IHubContext<SignalrSever> hubContext, ICampusService campusService)
        {
            _roomService = roomService;
            _hubContext = hubContext;
            _campusService = campusService;
        }

        [BindProperty]
        public Room Room { get; set; } = new Room();
        public List<Campus> Campuses { get; set; } = new List<Campus>();

        public void OnGet()
        {
            Campuses = _campusService.GetAllCampuses(null, null, null, null);
        }
        public IActionResult OnPost()
        {


            bool result = _roomService.Create(Room);

            if (result)
            {
                _hubContext.Clients.All.SendAsync("LoadAllRoom");
                return RedirectToPage("./RoomManagementPage");
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi tạo campus. Vui lòng thử lại.");
            return Page();
        }
    }
}
