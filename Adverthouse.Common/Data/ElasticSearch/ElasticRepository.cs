﻿using Adverthouse.Core.Configuration;
using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data.ElasticSearch
{
    public class ElasticRepository<T> : IElasticRepository<T> where T : class
    {
        private IElasticClient _elasticClient;
        public ElasticRepository(AppSettings appSettings)
        {
            var _elasticConfig = appSettings.ElasticSearchConfig;

            List<Uri> hosts = new List<Uri>();
            foreach (var host in _elasticConfig.HostAddresses.Split(","))
            {
                hosts.Add(new Uri(host));
            }
            var connectionPool = new StaticConnectionPool(hosts);

            var settings = new ConnectionSettings(connectionPool);
            if (_elasticConfig.EnableAuthentication)
            {
                settings = settings.BasicAuthentication(_elasticConfig.Username, _elasticConfig.Password);
            }

            if (_elasticConfig.EnableDebugMode) settings = settings.EnableDebugMode();

            _elasticClient = new ElasticClient(settings);
        }

        public ExistsResponse IsIndexExist(string indexName)
        {
            IIndexExistsRequest iier = new IndexExistsRequest(indexName);
            return _elasticClient.Indices.Exists(iier);
        }

        public CreateIndexResponse CreateIndex(string indexName, int numberOfReplica = 1, int numberOfShards = 5)
        {
            if (!IsIndexExist(indexName).Exists)
            {
                return _elasticClient.Indices.Create(indexName,
                        c => c.Map<T>(m => m.AutoMap())
                              .Settings(a => a.NumberOfReplicas(numberOfReplica)
                              .NumberOfShards(numberOfShards))
                );
            }
            else return null;
        }

        public BulkResponse UpsertDocument(string indexName, List<T> documents, bool autoCreateIndex = true)
        {
            if (autoCreateIndex) CreateIndex(indexName);

            return _elasticClient.Bulk(b => b
                   .UpdateMany(documents, (bc, d) => bc
                       .Doc(d)
                       .DocAsUpsert(true)
                   ).Refresh(Refresh.True)
                );
        }

        public DeleteIndexResponse DeleteIndex(string indexName)
        {

            if (!IsIndexExist(indexName).Exists)
            {
                IDeleteIndexRequest deleteIndexRequest = new DeleteIndexRequest(indexName);
                return _elasticClient.Indices.Delete(deleteIndexRequest);
            }
            else return null;
        }

        public ISearchResponse<T> Search(ElasticSearchBuilder elasticSearchBuilder)
        {
            ISearchRequest<T> searchRequest = new SearchRequest<T>();

            searchRequest.From = (elasticSearchBuilder.PSF.CurrentPage - 1) * elasticSearchBuilder.PSF.ItemPerPage;
            searchRequest.Size = elasticSearchBuilder.PSF.ItemPerPage;

            searchRequest.Query = elasticSearchBuilder.queryPreFilter;
            searchRequest.PostFilter = elasticSearchBuilder.queryPostFilter;

            if (elasticSearchBuilder.Sort != null)
                searchRequest.Sort = elasticSearchBuilder.Sort;

            return _elasticClient.Search<T>(searchRequest);
        }
    }
}