using Adverthouse.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Adverthouse.Common.Data
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity 
    {
        protected readonly DbContext _db;
        private readonly IDbContextFactory<DbContext> _contextFactory;

        public EntityRepository(DbContext db,
                                IDbContextFactory<DbContext> contextFactory)
        {
            _db = db;
            _contextFactory = contextFactory;
        }

        public bool IsMultiThreaded { get; set; } = false;

        public IQueryable<TEntity> GetResult()
        {
            if (IsMultiThreaded)
            {
                using (var _db2 = _contextFactory.CreateDbContext())
                {
                    return _db2.Set<TEntity>();
                }
            }
            else return _db.Set<TEntity>();
        }

        public virtual ListingResult<TEntity, PSF> GetResult<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase 
        {
            ListingResult<TEntity, PSF> opRes = new ListingResult<TEntity, PSF>();
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
        public virtual ListingResult<TEntity, PSF> GetResult<PSF>(PSF psfInfo) where PSF : IPSFBase
        {
            using (var _db2 = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> filteredQuery = _db2.Set<TEntity>();
                var opRes = GetResult(psfInfo, filteredQuery);
                return opRes;
            }
        }
        public List<TEntity> GetResult(Expression<Func<TEntity, bool>> predicate = null)
        {
            using (var _db2 = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> filteredQuery = _db2.Set<TEntity>();
                if (predicate != null)
                    filteredQuery = filteredQuery.Where(predicate);
                return filteredQuery.ToList();
            }
        }

        public List<int> SelectIDs(Expression<Func<TEntity, int?>> selectExp)
        {
            return _db.Set<TEntity>().Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToList();
        }
        public List<int> SelectIDs(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp)
        {
            return _db.Set<TEntity>().Where(whereExp).Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToList();
        }
        public List<int> SelectIDs<PSF>(PSF psfInfo, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp) where PSF : IPSFBase
        {
            var result = _db.Set<TEntity>().AsQueryable();

            if (whereExp != null) result = result.Where(whereExp);

            return result.OrderBy(psfInfo.SortExpression)
                             .Skip((psfInfo.CurrentPage - 1) * psfInfo.ItemPerPage)
                             .Take(psfInfo.ItemPerPage).Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToList();
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

        public void Dispose()
        {
            if (_db != null)  _db.Dispose();  
            GC.SuppressFinalize(this);
        }
    }
}