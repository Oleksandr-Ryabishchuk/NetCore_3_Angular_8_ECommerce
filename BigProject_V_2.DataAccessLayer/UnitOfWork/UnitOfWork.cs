using BigProject_V_2.DataAccessLayer.Data;
using BigProject_V_2.DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.DataAccessLayer.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        public ApplicationDbContext _applicationDbContext;
        private Dictionary<Type, object> repositories;
        private bool disposed;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            repositories = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _applicationDbContext.Dispose();

            disposed = true;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories.ContainsKey(typeof(TEntity)))
                return repositories[typeof(TEntity)] as IRepository<TEntity>;

            var repository = new Repository<TEntity>(_applicationDbContext);
            repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void Save()
        {
            try
            {
                _applicationDbContext.SaveChanges();
            }
            catch (ValidationException e)
            {

                throw new ValidationException(e.Message);
            }
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _applicationDbContext.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                throw new ValidationException(e.Message);
            }

        }
    }
}
