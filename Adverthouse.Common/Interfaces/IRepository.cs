using Adverthouse.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Adverthouse.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity 
    {
        IQueryable<TEntity> GetResult();
        ListingResult<TEntity, IPSFBase> GetResult(IPSFBase psfInfo, IQueryable<TEntity> preQuery);
        ListingResult<TEntity, IPSFBase> GetResult(IPSFBase psfInfo);
        void Add(TEntity entity);
        void AddIfNotExists(Expression<Func<TEntity, bool>> predicate, TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEntity>> include);
        int Count(Expression<Func<TEntity, bool>> predicate);
        int Count();        
        void Delete(Expression<Func<TEntity, bool>> criteria);
    }
}
