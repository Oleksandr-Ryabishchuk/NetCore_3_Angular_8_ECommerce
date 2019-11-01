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

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }  

        public async Task<T> AddAsync(T t)
        {
            await dbSet.AddAsync(t);
            
            return t;
        }       

        public void Delete(T entity)
        {
            dbSet.Remove(entity);           
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
    }
}

