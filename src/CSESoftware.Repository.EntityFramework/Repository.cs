using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSESoftware.Core.Entity;

namespace CSESoftware.Repository.EntityFramework
{
    public class Repository<TContext> : ReadOnlyRepository<TContext>, IRepository where TContext : DbContext
    {

        public Repository(TContext context) : base(context)
        {
        }

        public virtual void Create<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            entity.CreateSetup();
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Create<TEntity>(List<TEntity> entities)
            where TEntity : class, IEntity
        {
            foreach (var entity in entities)
                entity.CreateSetup();

            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            entity.UpdateSetup();
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update<TEntity>(List<TEntity> entities)
            where TEntity : class, IEntity
        {
            foreach (var entity in entities)
            {
                Context.Set<TEntity>().Attach(entity);
                entity.UpdateSetup();
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete<TEntity, T>(T id)
            where TEntity : class, IEntityWithId<T>
        {
            var entity = Context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            var dbSet = Context.Set<TEntity>();
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Delete<TEntity>(List<TEntity> entities)
            where TEntity : class, IEntity
        {
            foreach (var entity in entities.Where(entity => Context.Entry(entity).State == EntityState.Detached))
            {
                Context.Set<TEntity>().Attach(entity);
            }

            Context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntity
        {
            Context.Set<TEntity>().RemoveRange(Context.Set<TEntity>().Where(filter));
        }

        public virtual Task SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}