using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IRoomService
    {
        int GetTotal(string keyWord);
        List<Room> GetAll(string? keyWord, bool? isSortByName, int? pageIndex = null, int? pageSize = null);
        Room Get(int id);

        bool Create(Room model);

        bool Delete(Room model);
        List<Room> GetRooms();
    }
}
