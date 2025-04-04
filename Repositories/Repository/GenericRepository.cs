using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly BookingManagementContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        public GenericRepository(BookingManagementContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public IQueryable<TEntity> Entities => _dbSet;

        public bool Delete(object id)
        {
            TEntity? entity = _dbSet.Find(id);
            if (entity != null)
            {
                entity.GetType().GetProperty("DeletedAt")?.SetValue(entity, DateTime.Now);
                _dbSet.Update(entity);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        public bool Insert(TEntity obj)
        {
            _dbSet.Add(obj);
            return _dbContext.SaveChanges() > 0;
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public bool Update(TEntity obj)
        {
            _dbSet.Attach(obj);
            _dbSet.Entry(obj).State = EntityState.Modified;
            return _dbContext.SaveChanges() > 0;
        }


    }
}
