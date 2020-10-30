﻿using Adverthouse.Common.Data;
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
            ListingResult<TEntity, PSF> GetResult<PSF>(PSF psfInfo, IQueryable<TEntity> preQuery) where PSF : IPSFBase;
            ListingResult<TEntity, PSF> GetResult<PSF>(PSF psfInfo) where PSF : IPSFBase;
            List<int> SelectIDs(Expression<Func<TEntity, int?>> selectExp);
            List<int> SelectIDs(Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp);
            List<int> SelectIDs<PSF>(PSF psfInfo, Expression<Func<TEntity, bool>> whereExp, Expression<Func<TEntity, int?>> selectExp) where PSF : IPSFBase;
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
