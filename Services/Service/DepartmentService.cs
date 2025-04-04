using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Department> GetAll(string? keyWord, bool? isSortByName, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Department> query = _unitOfWork.DepartmentRepository
                .Entities.Where(c => c.DeletedAt == null);
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(keyWord.Trim().ToLower()) && p.DeletedAt == null);
            }
            if (isSortByName.HasValue)
            {
                query = isSortByName.Value ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
            }
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 20;
                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }
            return query.ToList();
        }

        public int GetTotal(string keyword)
        {
            IQueryable<Department> query = _unitOfWork.DepartmentRepository
                .Entities;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{keyword}%"));
            }

            return query.Count();
        }

        public Department Get(int id)
        {
            Department campus = (_unitOfWork.DepartmentRepository
                .Entities.Where(c => c.Id == id)).FirstOrDefault();
            return campus;
        }

        public bool Create(Department model)
        {
            return _unitOfWork.DepartmentRepository.Insert(model);
        }

        public bool Delete(Department model)
        {
            return _unitOfWork.DepartmentRepository.Delete(model.Id);
        }
    }
}