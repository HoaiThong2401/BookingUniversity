using BookingManagementRazorPages.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;
using Services.IService;

namespace BookingManagementRazorPages.Pages.Manager.CampusManagement
{
    public class DeleteCampusPageModel : PageModel
    {
        private readonly ICampusService _service;
        private readonly IHubContext<SignalrSever> _hubContext;

        public DeleteCampusPageModel(ICampusService service, IHubContext<SignalrSever> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Campus Campus { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            

            if (id == null)
            {
                return NotFound();
            }

            Campus campus = _service.GetCampus((int)id);

            if (campus == null)
            {
                return NotFound();
            }
            else
            {
                Campus = campus;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Campus campus = _service.GetCampus((int)id);
            if (campus != null)
            {
                Campus = campus;
                _service.DeleteCampus(campus);

                await _hubContext.Clients.All.SendAsync("LoadAllCampus");
            }

            return RedirectToPage("./CampusManagementPage");
        }
    }
}
