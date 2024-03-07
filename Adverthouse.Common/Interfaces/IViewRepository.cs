using Adverthouse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adverthouse.Common.Interfaces
{
    public interface IViewRepository<TViewModel>:IDisposable where TViewModel : class, IViewModel 
    {
        IQueryable<TViewModel> Queryable { get; }
        List<TViewModel> GetResult(Expression<Func<TViewModel, bool>> predicate = null);
        PagedList<TViewModel, PSF> GetResult<PSF>(PSF psfInfo, IQueryable<TViewModel> preQuery) where PSF : IPSFBase;
        PagedList<TViewModel, PSF> GetResult<PSF>(PSF psfInfo) where PSF : IPSFBase;            
        List<TVar> SelectIDs<TVar>(Expression<Func<TViewModel, TVar>> selectExp);
        List<TVar> SelectIDs<TVar>(Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp);
        List<TVar> SelectIDs<PSF,TVar>(PSF psfInfo, Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp) where PSF : IPSFBase;
        TViewModel FindBy(Expression<Func<TViewModel, bool>> predicate, bool enableLazyLoad = false);
        TViewModel FindBy(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IEnumerable<IViewModel>>> include, bool enableLazyLoad = false);
        TViewModel FindBy(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IViewModel>> include, bool enableLazyLoad = false);
        int Count(Expression<Func<TViewModel, bool>> predicate);
        int Count(); 
        Task<PagedList<TViewModel, PSF>> GetResultAsync<PSF>(PSF psfInfo, IQueryable<TViewModel> preQuery) where PSF : IPSFBase;
        Task<PagedList<TViewModel, PSF>> GetResultAsync<PSF>(PSF psfInfo) where PSF : IPSFBase;
        Task<List<TViewModel>> GetResultAsync(Expression<Func<TViewModel, bool>> predicate = null); 
        Task<TViewModel> FindByAsync(Expression<Func<TViewModel, bool>> predicate, bool enableLazyLoad = false);
        Task<TViewModel> FindByAsync(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IViewModel>> include, bool enableLazyLoad = false);
        Task<TViewModel> FindByAsync(Expression<Func<TViewModel, bool>> predicate, Expression<Func<TViewModel, IEnumerable<IViewModel>>> include, bool enableLazyLoad = false);
        Task<int> CountAsync(Expression<Func<TViewModel, bool>> predicate);
        Task<int> CountAsync(); 
        Task<List<TVar>> SelectIDsAsync<TVar>(Expression<Func<TViewModel, TVar>> selectExp);
        Task<List<TVar>> SelectIDsAsync<TVar>(Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp);
        Task<List<TVar>> SelectIDsAsync<PSF,TVar>(PSF psf, Expression<Func<TViewModel, bool>> whereExp, Expression<Func<TViewModel, TVar>> selectExp) where PSF : IPSFBase;
    }
}