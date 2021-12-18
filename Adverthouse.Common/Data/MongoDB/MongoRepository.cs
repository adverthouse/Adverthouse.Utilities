using Adverthouse.Core.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.MongoDB
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private IMongoCollection<TDocument> _collection;
        private readonly IMongoDatabase _database;
        private readonly MongoDBConfig _mongoDBConfig;
        public MongoRepository(AppSettings appSettings)
        {
            _mongoDBConfig = appSettings.MongoDBConfig;

            MongoClientSettings _clientSettings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(_mongoDBConfig.Host, 27017)
            };
            
            if (_mongoDBConfig.HasCredential)
            {
                _clientSettings.Credential =  MongoCredential.CreateCredential(
                        _mongoDBConfig.DBName,
                        _mongoDBConfig.Username,
                        _mongoDBConfig.Password);
            }

            _database = new MongoClient(_clientSettings).GetDatabase(_mongoDBConfig.DBName);
            _collection = _database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));

        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        private void ChangeCollection(string collectionName) {
            _collection = _database.GetCollection<TDocument>(collectionName);
        }
        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public virtual TDocument FindById<TFieldValue>(TFieldValue id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            return _collection.Find(filter).SingleOrDefault();
        }
        public virtual TDocument FindById<TFieldValue>(string collectionName,TFieldValue id)
        {
            ChangeCollection(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync<TFieldValue>(TFieldValue id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq("_id", id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }


        public async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void UpdateMany(FilterDefinition<TDocument> filter, 
            UpdateDefinition<TDocument> documents,
            bool isUpsert = false) =>
            _collection.UpdateMany(filter, documents,new UpdateOptions() { 
              IsUpsert =isUpsert
            });

        public ReplaceOneResult ReplaceOne<TFieldValue>(TFieldValue Id, TDocument document, 
            bool isUpsert = true)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", Id);
            return _collection.ReplaceOne(filter, document,new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public ReplaceOneResult ReplaceOne<TFieldValue>(string collectionName,TFieldValue Id, TDocument document, bool isUpsert = true)
        {
            ChangeCollection(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("_id", Id);
            return _collection.ReplaceOne(filter, document, new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public virtual async Task ReplaceOneAsync<TFieldValue>(TFieldValue Id, TDocument document, bool isUpsert = true)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", Id);
            await _collection.ReplaceOneAsync(filter, document, new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public long Count<TFieldValue>(string collectionName,TFieldValue Id){
            ChangeCollection(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("_id", Id); 
            return _collection.CountDocuments(filter);
        }

        public List<TFieldValue> GetIDs<TFieldValue>(string collectionName){
            ChangeCollection(collectionName);          
            var filterExpression = Builders<TDocument>.Filter.Ne("_id", "-1");  
            return _collection.Distinct<TFieldValue>("_id",filterExpression).ToList();
        }
        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById<TFieldValue>(TFieldValue id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            _collection.FindOneAndDelete(filter);
        }
        public void DeleteById<TFieldValue>(string collectionName, TFieldValue id)
        {
            ChangeCollection(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            _collection.FindOneAndDelete(filter);
        }
        public Task DeleteByIdAsync<TFieldValue>(TFieldValue id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq("_id", id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }

        public void DropCollection(string collectionName)
        {
            ChangeCollection(collectionName);             
            _database.DropCollection(collectionName);
        }

    }
}
