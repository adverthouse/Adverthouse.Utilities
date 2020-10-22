using Adverthouse.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Adverthouse.Common.Data
{
    public abstract class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity 
    {
        protected readonly DbContext _db;
        public EntityRepository(DbContext db)
        {
            _db = db;
        }
        public IQueryable<TEntity> GetResult()
        {
            return _db.Set<TEntity>();          
        }
        public virtual ListingResult<TEntity, IPSFBase> GetResult(IPSFBase psfInfo, IQueryable<TEntity> preQuery)
        {
            ListingResult<TEntity, IPSFBase> opRes = new ListingResult<TEntity, IPSFBase>();
            IQueryable<TEntity> filteredQuery = preQuery;

            if (psfInfo != null)
            { 
                opRes.PagingInfo = psfInfo;
                if (psfInfo.SetPageNumbers)
                {
                    psfInfo.TotalItemCount = filteredQuery.Count();
                }
            }

            opRes.Data = filteredQuery.OrderBy(psfInfo.SortExpression)
                             .Skip((psfInfo.CurrentPage - 1) * psfInfo.ItemPerPage)
                             .Take(psfInfo.ItemPerPage).ToList();

            return opRes;
        }
        public virtual ListingResult<TEntity, IPSFBase> GetResult(IPSFBase psfInfo)
        {
            IQueryable<TEntity> filteredQuery = _db.Set<TEntity>();
            var opRes = GetResult(psfInfo, filteredQuery);
            return opRes;
        }
        public List<TEntity> GetResult(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> filteredQuery = _db.Set<TEntity>();
            if (predicate != null)
                filteredQuery = filteredQuery.Where(predicate);
            return filteredQuery.ToList();
        }
        public virtual void Add(TEntity entity)
        {
            _db.Set<TEntity>().Add(entity);
            _db.SaveChanges();
        }
        public virtual void AddIfNotExists(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            if (Count(predicate) == 0)
            {
                Add(entity);
            }
        }
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().AddRange(entities);
            _db.SaveChanges();
        }
        public virtual void Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }
        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            _db.ChangeTracker.LazyLoadingEnabled = false;
            return _db.Set<TEntity>().Where(predicate).FirstOrDefault();
        }
        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include)
        {
            return _db.Set<TEntity>().Include(include).Where(predicate).FirstOrDefault();
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate).Count();
        }
        public int Count()
        {
            return _db.Set<TEntity>().Count();
        }        
        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            TEntity entity = FindBy(criteria);
            _db.Entry(entity).State = EntityState.Deleted;
            _db.SaveChanges();
        }

    }
}