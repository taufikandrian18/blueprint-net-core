using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Services.Interfaces;

namespace WITNetCoreProject.Services.Repositories {

    // this command is implementation repositories from base command in interface IBaseProfileRepository
    public abstract class BaseRepository<T> : IBaseProfileRepository<T> where T : class {

        protected RepositoryContext RepositoryContext { get; set; }

        public BaseRepository(RepositoryContext repositoryContext) {

            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll() {

            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) {

            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity) {

            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity) {

            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity) {

            this.RepositoryContext.Set<T>().Remove(entity);
        }
    }
}
