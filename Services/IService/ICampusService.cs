using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICampusService
    {
        bool CreateCampus(Campus campus);
        List<Campus> GetAllCampuses(string? keyWord, bool? isSortByName, int? pageIndex = null, int? pageSize = null);
        int GetTotalCampus(string? keyword);
        Campus GetCampus(int id);
        bool DeleteCampus(Campus campus);
    }
}
