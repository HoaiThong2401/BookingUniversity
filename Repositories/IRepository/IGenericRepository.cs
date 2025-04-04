using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Entities { get; }
        bool Insert(TEntity obj);
        bool Update(TEntity obj);
        bool Delete(object id);
        void Save();
    }
}
