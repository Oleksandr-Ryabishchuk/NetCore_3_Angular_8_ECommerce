using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.DataAccessLayer.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private DbSet<T> dbSet;
        private DbContext _dbContext;

       
        public Repository(DbContext dataContext)
        {
            dbSet = dataContext.Set<T>();
            _dbContext = dataContext;
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await dbSet.ToListAsync();
        }

        public virtual T Get(int id)
        {
            return dbSet.Find(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual T Add(T t)
        {

            dbSet.Add(t);
           
            return t;
        }

        public async Task<T> AddAsync(T t)
        {
            await dbSet.AddAsync(t);
            
            return t;

        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return dbSet.SingleOrDefault(match);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await dbSet.SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return dbSet.Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await dbSet.Where(match).ToListAsync();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);           
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;
            T exist = _dbContext.Set<T>().Find(key);
            if (exist != null)
            {
                _dbContext.Entry(exist).CurrentValues.SetValues(t);
            }
            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, object key)
        {
            if (t == null)
                return null;
            T exist = await _dbContext.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _dbContext.Entry(exist).State = EntityState.Modified;//.CurrentValues.SetValues(t);               
            }
            return exist;
        }

        public int Count()
        {
            return dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await dbSet.CountAsync();
        }
             
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = dbSet.Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }      
    }
}

