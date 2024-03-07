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

    public class ViewRepository<TViewModel> : IViewRepository<TViewModel> where TViewModel : class, IViewModel 
    {
        protected readonly DbContext _db;
        public ViewRepository(DbContext db)  => _db = db; 
        public IQueryable<TViewModel> Queryable => _db.Set<TViewModel>();
        
        public virtual PagedList<TViewModel, PSF> GetResult<PSF>(PSF psf, IQueryable<TViewModel> preQuery) where PSF : IPSFBase 
        {
            PagedList<TViewModel, PSF> opRes = new();
            IQueryable<TViewModel> filteredQuery = preQuery;

            if (psf != null)
            {
                opRes.PSF = psf;
                if (psf.SetPageNumbers)
                    psf.TotalItemCount = filteredQuery.AsNoTracking().Count();
            }

            opRes.Data = filteredQuery.OrderBy(psf.SortExpression)
                             .Skip((psf.CurrentPage - 1) * psf.ItemPerPage)
                             .Take(psf.ItemPerPage).AsNoTracking().ToList();

            return opRes;
        }

        public virtual async Task<PagedList<TViewModel, PSF>> GetResultAsync<PSF>(PSF psf, IQueryable<TViewModel> preQuery) where PSF : IPSFBase
        {
            PagedList<TViewModel, PSF> opRes = new();
            IQueryable<TViewModel> filteredQuery = preQuery;

            if (psf != null)
            {
                opRes.PSF = psf;
                if (psf.SetPageNumbers) 
                    psf.TotalItemCount = await filteredQuery.AsNoTracking().CountAsync();
            }

            opRes.Data = await filteredQuery.OrderBy(psf.SortExpression)
                             .Skip((psf.CurrentPage - 1) * psf.ItemPerPage)
                             .Take(psf.ItemPerPage).AsNoTracking().ToListAsync();

            return opRes;
        }

        public virtual PagedList<TViewModel, PSF> GetResult<PSF>(PSF psf) where PSF : IPSFBase
        {
            IQueryable<TViewModel> filteredQuery = _db.Set<TViewModel>();
            var opRes = GetResult(psf, filteredQuery);
            return opRes;
        }
        public virtual async Task<PagedList<TViewModel, PSF>> GetResultAsync<PSF>(PSF psf) where PSF : IPSFBase
        {
            IQueryable<TViewModel> filteredQuery = _db.Set<TViewModel>();
            var opRes = await GetResultAsync(psf, filteredQuery);
            return opRes;
        }

        public List<TViewModel> GetResult(Expression<Func<TViewModel, bool>> predicate = null)
        { 
            IQueryable<TViewModel> filteredQuery = _db.Set<TViewModel>();
            if (predicate != null)
                filteredQuery = filteredQuery.Where(predicate);
            return filteredQuery.AsNoTracking().ToList();
        }

        public async Task<List<TViewModel>> GetResultAsync(Expression<Func<TViewModel, bool>> predicate = null)
        {
            IQueryable<TViewModel> filteredQuery = _db.Set<TViewModel>();
            if (predicate != null)
                filteredQuery = filteredQuery.Where(predicate);
            return await filteredQuery.AsNoTracking().ToListAsync();
        }

        public List<TVar> SelectIDs<TVar>(Expression<Func<TViewModel, TVar>> selectExp)
        {
            return _db.Set<TViewModel>().AsNoTracking().Select(selectExp).Distinct().Where(x => x != null).Cast<TVar>().ToList();
        }

        public async Task<List<TVar>> SelectIDsAsync<TVar>(Expression<Func<TViewModel, TVar>> selectExp)
        {
            return await _db.Set<TViewModel>().AsNoTracking().Select(selectExp).Distinct().Where(x => x != null).Cast<TVar>().ToListAsync();
        }
        public List<TVar> SelectIDs<TVar>(Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp)
        {
            return _db.Set<TViewModel>().AsNoTracking().Where(whereExp).Select(selectExp).Distinct().Where(x => x != null).Cast<TVar>().ToList();
        }
        public async Task<List<TVar>> SelectIDsAsync<TVar>(Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp)
        {
            return await _db.Set<TViewModel>().AsNoTracking().Where(whereExp).Select(selectExp).Distinct().Where(x => x != null).Cast<TVar>().ToListAsync();
        }
        public List<TVar> SelectIDs<PSF, TVar>(PSF psf, Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp) where PSF : IPSFBase
        {
            var result = _db.Set<TViewModel>().AsQueryable();

            if (whereExp != null) result = result.Where(whereExp);

            return result.OrderBy(psf.SortExpression)
                             .Skip((psf.CurrentPage - 1) * psf.ItemPerPage)
                             .Take(psf.ItemPerPage).AsNoTracking().Select(selectExp).Distinct().Where(x => x != null).Cast<TVar>().ToList();
        }

        public async Task<List<TVar>> SelectIDsAsync<PSF, TVar>(PSF psf, Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp) where PSF : IPSFBase
        {
            var result = _db.Set<TViewModel>().AsQueryable();

            if (whereExp != null) result = result.Where(whereExp);

            return await result.OrderBy(psf.SortExpression)
                             .Skip((psf.CurrentPage - 1) * psf.ItemPerPage)
                             .Take(psf.ItemPerPage).AsNoTracking().Select(selectExp).Distinct().Where(x => x != null).Cast<TVar>().ToListAsync();
        } 

        public TViewModel FindBy(Expression<Func<TViewModel, bool>> predicate, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return _db.Set<TViewModel>().Where(predicate).FirstOrDefault(); 
        }

        public async Task<TViewModel> FindByAsync(Expression<Func<TViewModel, bool>> predicate, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return await _db.Set<TViewModel>().Where(predicate).FirstOrDefaultAsync();
        }
        public TViewModel FindBy(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IViewModel>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return _db.Set<TViewModel>().Include(include).Where(predicate).FirstOrDefault();
        }
        public async Task<TViewModel> FindByAsync(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IViewModel>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return await _db.Set<TViewModel>().Include(include).Where(predicate).FirstOrDefaultAsync();
        }
        public TViewModel FindBy(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IEnumerable<IViewModel>>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return _db.Set<TViewModel>().Include(include).Where(predicate).FirstOrDefault();
        }

        public async Task<TViewModel> FindByAsync(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IEnumerable<IViewModel>>> include, bool enableLazyLoad = false)
        {
            _db.ChangeTracker.LazyLoadingEnabled = enableLazyLoad;
            return await _db.Set<TViewModel>().Include(include).Where(predicate).FirstOrDefaultAsync();
        } 

        public int Count(Expression<Func<TViewModel, bool>> predicate)
        {
            return _db.Set<TViewModel>().Where(predicate).Count();
        }
        public async Task<int> CountAsync(Expression<Func<TViewModel, bool>> predicate)
        {
            return await _db.Set<TViewModel>().Where(predicate).CountAsync();
        }
        public int Count()
        {
            return _db.Set<TViewModel>().Count();
        }
        public async Task<int> CountAsync()
        {
            return await _db.Set<TViewModel>().CountAsync();
        } 
        public void Dispose()
        {
            if (_db != null)  _db.Dispose();  
            GC.SuppressFinalize(this);
        }
    }
}