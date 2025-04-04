using BookingManagementRazorPages.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repositories.Entities;
using Services.IService;

namespace BookingManagementRazorPages.Pages.Manager.DepartmentManagement
{
    public class CreateDepartmentPageModel : PageModel
    {
        private readonly IDepartmentService _departmentService;
        private readonly ICampusService _campusService;
        private readonly IHubContext<SignalrSever> _hubContext;

        public CreateDepartmentPageModel(IDepartmentService departmentService, IHubContext<SignalrSever> hubContext, ICampusService campusService)
        {
            _departmentService = departmentService;
            _hubContext = hubContext;
            _campusService = campusService;
        }

        [BindProperty]
        public Department Department { get; set; } = new Department();
        public List<Campus> Campuses { get; set; } = new List<Campus>();

        public void OnGet()
        {
            Campuses = _campusService.GetAllCampuses(null, null, null, null);
        }
        public IActionResult OnPost()
        {
            Campus campus = _campusService.GetCampus(Department.CampusId);

            Department.Code = $"{Department.Name}-{campus.Code}";
            

            bool result = _departmentService.Create(Department);

            if (result)
            {
                _hubContext.Clients.All.SendAsync("LoadAllDepartment");
                return RedirectToPage("./DepartmentManagementPage");
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi tạo campus. Vui lòng thử lại.");
            return Page();
        }
    }
}
