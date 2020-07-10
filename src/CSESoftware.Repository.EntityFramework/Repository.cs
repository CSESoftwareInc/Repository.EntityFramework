using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSESoftware.Core.Entity;
using CSESoftware.Repository.EntityFramework.Helpers;

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
            if (entity is IActiveEntity activeEntity)
                entity = PrepareService.SetIsActiveToTrue<TEntity>(activeEntity);

            if (entity is IModifiedEntity modifiedEntity)
                entity = PrepareService.SetCreatedDateToNow<TEntity>(modifiedEntity);

            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Create<TEntity>(List<TEntity> entities)
            where TEntity : class, IEntity
        {
            var entitiesToSave = new List<TEntity>();

            foreach (var entity in entities)
            {
                var tempEntity = entity;

                if (entity is IActiveEntity activeEntity)
                    tempEntity = PrepareService.SetIsActiveToTrue<TEntity>(activeEntity);

                if (entity is IModifiedEntity modifiedEntity)
                    tempEntity = PrepareService.SetCreatedDateToNow<TEntity>(modifiedEntity);

                entitiesToSave.Add(tempEntity);
            }

            Context.Set<TEntity>().AddRange(entitiesToSave);
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            if (entity is IModifiedEntity modifiedEntity)
                entity = PrepareService.SetModifiedDateToNow<TEntity>(modifiedEntity);
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update<TEntity>(List<TEntity> entities)
            where TEntity : class, IEntity
        {
            var entitiesToSave = new List<TEntity>();

            foreach (var entity in entities)
            {
                var tempEntity = entity;

                if (entity is IModifiedEntity modifiedEntity)
                    tempEntity = PrepareService.SetModifiedDateToNow<TEntity>(modifiedEntity);

                entitiesToSave.Add(tempEntity);
            }

            foreach (var entity in entitiesToSave)
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete<TEntity>(object id)
            where TEntity : class, IEntityWithId
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