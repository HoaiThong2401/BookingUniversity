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
    public class CampusService : ICampusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateCampus(Campus campus)
        {
            return _unitOfWork.CampusRepository.Insert(campus);
        }

        public bool DeleteCampus(Campus campus)
        {
            return _unitOfWork.CampusRepository.Delete(campus.Id);
        }

        public List<Campus> GetAllCampuses(string? keyWord, bool? isSortByName, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Campus> query = _unitOfWork.CampusRepository
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

        public Campus GetCampus(int id)
        {
            Campus campus = (_unitOfWork.CampusRepository
                .Entities.Where(c => c.Id == id)).FirstOrDefault();
            return campus;
        }

        public int GetTotalCampus(string? keyword)
        {
            IQueryable<Campus> query = _unitOfWork.CampusRepository
                .Entities;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{keyword}%") || EF.Functions.Like(c.Location, $"%{keyword}%"));
            }

            return query.Count();
        }

    }
}