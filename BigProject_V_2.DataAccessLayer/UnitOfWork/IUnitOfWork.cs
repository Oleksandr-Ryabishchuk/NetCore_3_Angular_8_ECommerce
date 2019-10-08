using BigProject_V_2.DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void Save();
        Task<int> SaveAsync();
    }
}
