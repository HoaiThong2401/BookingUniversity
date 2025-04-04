//using BookingManagementRazorPages.Hubs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.SignalR;
//using Repositories.Entities;
//using Services.IService;

//namespace BookingManagementRazorPages.Pages.Manager.RoomManagement
//{
//    public class DeleteRoomPageModel : PageModel
//    {
//        private readonly IRoomService _service;
//        private readonly IHubContext<SignalrSever> _hubContext;

//        public DeleteRoomPageModel(IRoomService service, IHubContext<SignalrSever> hubContext)
//        {
//            _service = service;
//            _hubContext = hubContext;
//        }

//        [BindProperty]
//        public Room Room { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {


//            if (id == null)
//            {
//                return NotFound();
//            }

//            Room room = _service.Get((int)id);

//            if (room == null)
//            {
//                return NotFound();
//            }
//            else
//            {
//                Room = room;
//            }
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync(int? id)
//        {

//            if (id == null)
//            {
//                return NotFound();
//            }

//            Room room = _service.Get((int)id);
//            if (room != null)
//            {
//                Room = room;
//                _service.Delete(room);

//                _hubContext.Clients.All.SendAsync("LoadAllRoom");

//            }

//            return RedirectToPage("./RoomManagementPage");
//        }
//    }
//}
