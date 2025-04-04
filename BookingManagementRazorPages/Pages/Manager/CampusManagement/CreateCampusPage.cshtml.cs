using BookingManagementRazorPages.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;

using Services.IService;

namespace BookingManagementRazorPages.Pages.Manager.CampusManagement
{
    public class CreateCampusPageModel : PageModel
    {
        private readonly ICampusService _campusService;
        private readonly IHubContext<SignalrSever> _hubContext;

        public CreateCampusPageModel(ICampusService campusService, IHubContext<SignalrSever> hubContext)
        {
            _campusService = campusService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Campus Campus { get; set; } = new Campus();

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = _campusService.CreateCampus(Campus);

            if (result)
            {
                _hubContext.Clients.All.SendAsync("LoadAllDepartment");
                return RedirectToPage("./CampusManagementPage");
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi tạo campus. Vui lòng thử lại.");
            return Page();
        }

    }
}
