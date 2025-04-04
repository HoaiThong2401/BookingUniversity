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
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Room> GetAll(string? keyWord, bool? isSortByName, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Room> query = _unitOfWork.RoomRepository
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
            IQueryable<Room> query = _unitOfWork.RoomRepository
                .Entities;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{keyword}%"));
            }

            return query.Count();
        }

        public Room Get(int id)
        {
            Room campus = (_unitOfWork.RoomRepository
                .Entities.Where(c => c.Id == id)).FirstOrDefault();
            return campus;
        }

        public bool Create(Room model)
        {
            return _unitOfWork.RoomRepository.Insert(model);
        }

        public bool Delete(Room model)
        {
            return _unitOfWork.RoomRepository.Delete(model.Id);
        }
        public List<Room> GetRooms()
        {
            var approvedBookingDetailIds = _unitOfWork.ApprovalHistoryRepository.Entities
                .Where(ah => ah.ApprovalByManager == true)
                .Select(ah => ah.BookingDetailId)
                .Distinct()
                .ToList();

            var roomIds = _unitOfWork.BookingDetailRepository.Entities
                .Where(bd => approvedBookingDetailIds.Contains(bd.Id) && bd.DeletedAt == null)
                .Select(bd => bd.RoomId)
                .Distinct()
                .ToList();

            List<Room> room = new List<Room>();

            if (roomIds.Count == 0)
            {
                room = _unitOfWork.RoomRepository.Entities
                    .Where(r => r.DeletedAt == null)
                    .ToList();
            }
            else
            {
                room = _unitOfWork.RoomRepository.Entities
                    .Where(r => roomIds.Contains(r.Id) && r.DeletedAt == null)
                    .ToList();
            }

            return room;
        }

    }
}
