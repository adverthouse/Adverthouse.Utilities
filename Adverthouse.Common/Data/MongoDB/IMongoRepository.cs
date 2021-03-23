using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.MongoDB
{
    public interface IMongoRepository<TDocument> where TDocument  : IDocument
    {
        IQueryable<TDocument> AsQueryable();

        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        TDocument FindById<TFieldValue>(TFieldValue id);
        TDocument FindById<TFieldValue>(string collectionName, TFieldValue id);
        Task<TDocument> FindByIdAsync<TFieldValue>(TFieldValue id);

        void InsertOne(TDocument document);

        Task InsertOneAsync(TDocument document);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(ICollection<TDocument> documents);
        void ReplaceOne<TFieldValue>(string collectionName, TFieldValue Id, TDocument document, bool isUpsert = true);
        void ReplaceOne<TFieldValue>(TFieldValue Id, TDocument document, bool isUpsert = true);

        Task ReplaceOneAsync<TFieldValue>(TFieldValue Id, TDocument document, bool isUpsert = true);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        void DeleteById<TFieldValue>(TFieldValue id);
        void DeleteById<TFieldValue>(string collectionName, TFieldValue id);
        Task DeleteByIdAsync<TFieldValue>(TFieldValue id);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
        void DropCollection(string collectionName);
        void UpdateMany(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> documents, bool isUpsert = false);
    }
}
