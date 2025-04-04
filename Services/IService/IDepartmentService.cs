using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IDepartmentService
    {
        int GetTotal(string keyWord);
        List<Department> GetAll(string? keyWord, bool? isSortByName, int? pageIndex = null, int? pageSize = null);
        Department Get(int id);

        bool Create(Department model);

        bool Delete(Department model);
    }
}
