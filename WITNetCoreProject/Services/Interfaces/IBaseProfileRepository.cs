using System;
using System.Linq;
using System.Linq.Expressions;

namespace WITNetCoreProject.Services.Interfaces {

    // this interface is to make base command which going to use by all services to make command to database
    public interface IBaseProfileRepository<T> {

        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
