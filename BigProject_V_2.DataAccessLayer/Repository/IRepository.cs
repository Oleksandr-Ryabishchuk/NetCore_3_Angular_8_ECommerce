using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.DataAccessLayer.Repository
{
    public interface IRepository<T> where T: class
    {       
        Task<T> AddAsync(T t);             
        void Delete(T entity);      
        Task<IEnumerable<T>> GetAllAsync();     
        Task<T> GetAsync(int id);     
        Task<T> UpdateAsync(T t, object key);
    }
}
