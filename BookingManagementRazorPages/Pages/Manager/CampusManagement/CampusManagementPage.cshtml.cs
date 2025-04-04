using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Entities;
using Services.IService;

namespace BookingManagementRazorPages.Pages.Manager.CampusManagement
{
    public class CampusManagementPageModel : PageModel
    {
        private readonly ICampusService _campusService;

        public CampusManagementPageModel(ICampusService campusService)
        {
            _campusService = campusService;
        }

        public IList<Campus> Campuses { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public IActionResult OnGet(string? keyWord, string? sortOption, int? pageIndex, int pageSize = 2)
        {
            bool? sortByName = null;

            if (!string.IsNullOrEmpty(sortOption))
            {
                switch (sortOption)
                {
                    case "name_asc":
                        sortByName = true; break;
                    case "name_desc":
                        sortByName = false; break;
                }
            }

            int totalProducts = _campusService.GetTotalCampus(keyWord);
            TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            CurrentPage = pageIndex ?? 1;
            CurrentPage = Math.Max(1, Math.Min(CurrentPage, TotalPages));
            Campuses = _campusService.GetAllCampuses(keyWord, sortByName, CurrentPage, pageSize);

            return Page();
        }
    }
}
