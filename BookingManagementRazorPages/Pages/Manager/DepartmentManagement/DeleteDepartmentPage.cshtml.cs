using BookingManagementRazorPages.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;
using Services.IService;

namespace BookingManagementRazorPages.Pages.Manager.DepartmentManagement
{
    public class DeleteDepartmentPageModel : PageModel
    {
        private readonly IDepartmentService _service;
        private readonly IHubContext<SignalrSever> _hubContext;

        public DeleteDepartmentPageModel(IDepartmentService service, IHubContext<SignalrSever> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            Department department = _service.Get((int)id);

            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Department department = _service.Get((int)id);
            if (department != null)
            {
                Department = department;
                _service.Delete(department);

                _hubContext.Clients.All.SendAsync("LoadAllDepartment");
                
            }

            return RedirectToPage("./DepartmentManagementPage");
        }
    }
}
