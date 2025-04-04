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
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Slot> GetSlots()
        {

            List<Slot> slot = new List<Slot>();
            slot = _unitOfWork.SlotRepository.Entities.
                Where(p => p.DeletedAt == null)
                .ToList();
            return slot;
        }
    }
}
