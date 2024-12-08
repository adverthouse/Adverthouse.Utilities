using Adverthouse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adverthouse.Common.Interfaces
{
    public interface IRepository<TEntity>:IDisposable where TEntity : class, IEntity 
    {
        IQueryable<TEntity> Queryable { get; }
        IQueryable<TEntity> GetPagedListQueryable<PSF>(PSF psf) where PSF : IPSFBase; 
        List<TEntity> GetResult(Expression<Func<TEntity, bool>> predicate = null);
        PagedList<TEntity, PSF> GetResult<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase;
        PagedList<TEntity, PSF> GetResult<PSF>(PSF psfInfo) where PSF : IPSFBase;            
        List<TVar> SelectIDs<TVar>(Expression<Func<TEntity, TVar>> selectExp);
        List<TVar> SelectIDs<TVar>(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, TVar>> selectExp);
        List<TVar> SelectIDs<PSF,TVar>(PSF psfInfo, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, TVar>> selectExp) where PSF : IPSFBase;
        int Add(TEntity entity);
        int AddIfNotExists(Expression<Func<TEntity, bool>> predicate, TEntity entity);
        int AddRange(IEnumerable<TEntity> entities);
        int Update(TEntity entity);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, bool enableLazyLoad = false,bool noTracking = false);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<IEntity>>> include, bool enableLazyLoad = false,bool noTracking = false);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include, bool enableLazyLoad = false,bool noTracking = false);
        int Count(Expression<Func<TEntity, bool>> predicate);
        int Count();
        int Delete(Expression<Func<TEntity, bool>> criteria);
        Task<PagedList<TEntity, PSF>> GetResultAsync<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase;
        Task<PagedList<TEntity, PSF>> GetResultAsync<PSF>(PSF psfInfo) where PSF : IPSFBase;
        Task<List<TEntity>> GetResultAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> AddIfNotExistsAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity); 
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<int> UpdateAsync(TEntity entity);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, bool enableLazyLoad = false,bool noTracking = false);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include, bool enableLazyLoad = false,bool noTracking = false);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<IEntity>>> include, bool enableLazyLoad = false,bool noTracking = false);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> criteria);
        Task<List<TVar>> SelectIDsAsync<TVar>(Expression<Func<TEntity, TVar>> selectExp);
        Task<List<TVar>> SelectIDsAsync<TVar>(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, TVar>> selectExp);
        Task<List<TVar>> SelectIDsAsync<PSF,TVar>(PSF psf, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, TVar>> selectExp) where PSF : IPSFBase;
    }
    }
