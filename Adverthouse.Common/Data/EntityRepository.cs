using Adverthouse.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data
{

    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext _db;

        public EntityRepository(DbContext db)  => _db = db; 

        public IQueryable<TEntity> GetResult() => _db.Set<TEntity>();
        
        public virtual PagedList<TEntity, PSF> GetResult<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase 
        {
            PagedList<TEntity, PSF> opRes = new();
            IQueryable<TEntity> filteredQuery = preQuery;

            if (psfInfo != null)
            {
                opRes.PagingInfo = psfInfo;
                if (psfInfo.SetPageNumbers)
                    psfInfo.TotalItemCount = filteredQuery.Count();
            }

            opRes.Data = filteredQuery.OrderBy(psfInfo.SortExpression)
                             .Skip((psfInfo.CurrentPage - 1) * psfInfo.ItemPerPage)
                             .Take(psfInfo.ItemPerPage).ToList();

            return opRes;
        }

        public virtual async Task<PagedList<TEntity, PSF>> GetResultAsync<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase
        {
            PagedList<TEntity, PSF> opRes = new();
            IQueryable<TEntity> filteredQuery = preQuery;

            if (psfInfo != null)
            {
                opRes.PagingInfo = psfInfo;
                if (psfInfo.SetPageNumbers) 
                    psfInfo.TotalItemCount = await filteredQuery.CountAsync();
            }

            opRes.Data = await filteredQuery.OrderBy(psfInfo.SortExpression)
                             .Skip((psfInfo.CurrentPage - 1) * psfInfo.ItemPerPage)
                             .Take(psfInfo.ItemPerPage).ToListAsync();

            return opRes;
        }

        public virtual PagedList<TEntity, PSF> GetResult<PSF>(PSF psfInfo) where PSF : IPSFBase
        {
            IQueryable<TEntity> filteredQuery = _db.Set<TEntity>();
            var opRes = GetResult(psfInfo, filteredQuery);
            return opRes;
        }
        public virtual async Task<PagedList<TEntity, PSF>> GetResultAsync<PSF>(PSF psfInfo) where PSF : IPSFBase
        {
            IQueryable<TEntity> filteredQuery = _db.Set<TEntity>();
            var opRes = await GetResultAsync(psfInfo, filteredQuery);
            return opRes;
        }

        public List<TEntity> GetResult(Expression<Func<TEntity, bool>> predicate = null)
        { 
            IQueryable<TEntity> filteredQuery = _db.Set<TEntity>();
            if (predicate != null)
                filteredQuery = filteredQuery.Where(predicate);
            return filteredQuery.ToList();
        }

        public async Task<List<TEntity>> GetResultAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> filteredQuery = _db.Set<TEntity>();
            if (predicate != null)
                filteredQuery = filteredQuery.Where(predicate);
            return await filteredQuery.ToListAsync();
        }

        public List<int> SelectIDs(Expression<Func<TEntity, int?>> selectExp)
        {
            return _db.Set<TEntity>().Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToList();
        }

        public async Task<List<int>> SelectIDsAsync(Expression<Func<TEntity, int?>> selectExp)
        {
            return await _db.Set<TEntity>().Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToListAsync();
        }
        public List<int> SelectIDs(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp)
        {
            return _db.Set<TEntity>().Where(whereExp).Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToList();
        }
        public async Task<List<int>> SelectIDsAsync(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp)
        {
            return await _db.Set<TEntity>().Where(whereExp).Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToListAsync();
        }
        public List<int> SelectIDs<PSF>(PSF psfInfo, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp) where PSF : IPSFBase
        {
            var result = _db.Set<TEntity>().AsQueryable();

            if (whereExp != null) result = result.Where(whereExp);

            return result.OrderBy(psfInfo.SortExpression)
                             .Skip((psfInfo.CurrentPage - 1) * psfInfo.ItemPerPage)
                             .Take(psfInfo.ItemPerPage).Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToList();
        }
        public async Task<List<int>> SelectIDsAsync<PSF>(PSF psfInfo, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp) where PSF : IPSFBase
        {
            var result = _db.Set<TEntity>().AsQueryable();

            if (whereExp != null) result = result.Where(whereExp);

            return await result.OrderBy(psfInfo.SortExpression)
                             .Skip((psfInfo.CurrentPage - 1) * psfInfo.ItemPerPage)
                             .Take(psfInfo.ItemPerPage).Select(selectExp).Distinct().Where(x => x != null).Cast<int>().ToListAsync();
        }
        public virtual int Add(TEntity entity)
        {
            _db.Set<TEntity>().Add(entity);
            return _db.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await _db.Set<TEntity>().AddAsync(entity);
            return await _db.SaveChangesAsync();
        }
        public virtual int AddIfNotExists(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            if (Count(predicate) == 0) return Add(entity);
            return 0;
        }
        public virtual async Task<int> AddIfNotExistsAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            if (Count(predicate) == 0) return await AddAsync(entity);
            return 0;
        }
        public virtual int AddRange(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().AddRange(entities);
            return _db.SaveChanges();
        }
        public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _db.Set<TEntity>().AddRangeAsync(entities);
            return await _db.SaveChangesAsync();
        }
        public virtual int Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            return _db.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            return await _db.SaveChangesAsync();
        }

        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return _db.Set<TEntity>().Where(predicate).FirstOrDefault(); 
        }

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return await _db.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }
        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return _db.Set<TEntity>().Include(include).Where(predicate).FirstOrDefault();
        }
        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return await _db.Set<TEntity>().Include(include).Where(predicate).FirstOrDefaultAsync();
        }
        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<IEntity>>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return _db.Set<TEntity>().Include(include).Where(predicate).FirstOrDefault();
        }

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<IEntity>>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return await _db.Set<TEntity>().Include(include).Where(predicate).FirstOrDefaultAsync();
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate).Count();
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().Where(predicate).CountAsync();
        }
        public int Count()
        {
            return _db.Set<TEntity>().Count();
        }
        public async Task<int> CountAsync()
        {
            return await _db.Set<TEntity>().CountAsync();
        }
        public int Delete(Expression<Func<TEntity, bool>> criteria)
        {
            TEntity entity = FindBy(criteria);
            _db.Entry(entity).State = EntityState.Deleted;
            return _db.SaveChanges();
        }
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> criteria)
        {
            TEntity entity = FindBy(criteria);
            _db.Entry(entity).State = EntityState.Deleted;
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_db != null)  _db.Dispose();  
            GC.SuppressFinalize(this);
        }
    }
}