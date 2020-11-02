﻿using Adverthouse.Core.Configuration;
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
        public MongoRepository(MongoDBConfig mongoDBConfig,string collectionName)
        {

            MongoClientSettings _clientSettings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(mongoDBConfig.Host, 27017)
            };
            
            if (mongoDBConfig.HasCredential)
            {
                _clientSettings.Credentials = new List<MongoCredential>() {
                    MongoCredential.CreateMongoCRCredential(
                        mongoDBConfig.UserDB,
                        mongoDBConfig.Username,
                        mongoDBConfig.Password) };
            }

            _database = new MongoClient(_clientSettings).GetDatabase(mongoDBConfig.DBName);
            _collection = _database.GetCollection<TDocument>(collectionName);
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

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
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

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne<TFieldValue>(TFieldValue Id, TDocument document, bool isUpsert = true)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", Id);
            _collection.ReplaceOne(filter, document,new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public void ReplaceOne<TFieldValue>(string collectionName,TFieldValue Id, TDocument document, bool isUpsert = true)
        {
            ChangeCollection(collectionName);
            var filter = Builders<TDocument>.Filter.Eq("_id", Id);
            _collection.ReplaceOne(filter, document, new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public virtual async Task ReplaceOneAsync<TFieldValue>(TFieldValue Id, TDocument document, bool isUpsert = true)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", Id);
            await _collection.ReplaceOneAsync(filter, document, new ReplaceOptions() { IsUpsert = isUpsert });
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