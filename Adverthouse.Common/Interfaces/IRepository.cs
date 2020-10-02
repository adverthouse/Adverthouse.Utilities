using Adverthouse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Adverthouse.Common.Interfaces
{
    public interface IRepository<T, PSF> where T : class, IEntity where PSF : IPSFBase
    {
        ListingResult<T, PSF> GetResult(PSF psfInfo, IQueryable<T> preQuery);
        ListingResult<T, PSF> GetResult(PSF psfInfo);
        void Add(T entity);
        void Update(T entity);
        T FindBy(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        int Count();
        void Delete(Expression<Func<T, bool>> criteria);
    }
}
