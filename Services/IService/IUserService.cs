using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IUserService
    {
        User? GetAccountMemberByEmail(string email, string password);
        Task<int> GetDepartmentIdAsync(int userId);
    }
}
