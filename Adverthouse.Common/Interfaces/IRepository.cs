﻿using Adverthouse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adverthouse.Common.Interfaces
{
    public interface IRepository<TEntity>:IDisposable where TEntity : class, IEntity 
    {
        IQueryable<TEntity> GetResult();
        List<TEntity> GetResult(Expression<Func<TEntity, bool>> predicate = null);
        PagedList<TEntity, PSF> GetResult<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase;
        PagedList<TEntity, PSF> GetResult<PSF>(PSF psfInfo) where PSF : IPSFBase;            
        List<int> SelectIDs(Expression<Func<TEntity, int?>> selectExp);
        List<int> SelectIDs(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp);
        List<int> SelectIDs<PSF>(PSF psfInfo, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp) where PSF : IPSFBase;
        int Add(TEntity entity);
        int AddIfNotExists(Expression<Func<TEntity, bool>> predicate, TEntity entity);
        int AddRange(IEnumerable<TEntity> entities);
        int Update(TEntity entity);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, bool enableLazyLoad = false);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<IEntity>>> include, bool enableLazyLoad = false);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include, bool enableLazyLoad = false);
        int Count(Expression<Func<TEntity, bool>> predicate);
        int Count();
        int Delete(Expression<Func<TEntity, bool>> criteria);
        Task<PagedList<TEntity, PSF>> GetResultAsync<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase;
        Task<PagedList<TEntity, PSF>> GetResultAsync<PSF>(PSF psfInfo) where PSF : IPSFBase;
        Task<List<TEntity>> GetResultAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> AddIfNotExistsAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity); 
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<int> UpdateAsync(TEntity entity);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, bool enableLazyLoad = false);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include, bool enableLazyLoad = false);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<IEntity>>> include, bool enableLazyLoad = false);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> criteria);
        Task<List<int>> SelectIDsAsync(Expression<Func<TEntity, int?>> selectExp);
        Task<List<int>> SelectIDsAsync(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp);
        Task<List<int>> SelectIDsAsync<PSF>(PSF psf, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp) where PSF : IPSFBase;
    }
    }
