using Repositories.Entities;
using Repositories.IRepository;
using Services.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User? GetAccountMemberByEmail(string email, string password)
        {
            return _unitOfWork.UserRepository.Entities
                .FirstOrDefault(p => p.Email != null
                     && p.Email.Equals(email)
                     && p.Password != null
                     && p.Password.Equals(password));
        }
        public async Task<int> GetDepartmentIdAsync(int userId)
        {
            var roleId = await _unitOfWork.UserRepository
            .Entities
            .Where(u => u.Id == userId)
            .Select(u => u.DepartmentId)
            .FirstOrDefaultAsync();

            if (roleId == 0)
            {
                throw new Exception("User not found");
            }

            return roleId;
        }
    }
}
