using Adverthouse.Core.Configuration;
using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adverthouse.Common.Data.ElasticSearch
{
    public class ElasticRepository<T> : IElasticRepository<T> where T : class
    {
        private IElasticClient _elasticClient;
        public ElasticRepository(AppSettings appSettings)
        {
            var elasticConfig = appSettings.ElasticSearchConfig;

            var hosts = elasticConfig.HostAddresses
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(h => new Uri(h))
                .ToList();

            IConnectionPool connectionPool = hosts.Count == 1
                ? new SingleNodeConnectionPool(hosts.First())
                : new StaticConnectionPool(hosts);

            var settings = new ConnectionSettings(connectionPool)
                .RequestTimeout(TimeSpan.FromMinutes(5)); // increase if queries are heavy

            if (elasticConfig.EnableAuthentication)
            {
                settings = settings.BasicAuthentication(elasticConfig.Username, elasticConfig.Password);
            }

            if (elasticConfig.EnableDebugMode)
            {
                settings = settings.EnableDebugMode();
            }

            _elasticClient = new ElasticClient(settings);
        }

        public ExistsResponse IsIndexExist(string indexName)
        {
            return _elasticClient.Indices.Exists(new IndexExistsRequest(indexName));
        }

        public CreateIndexResponse CreateIndex(string indexName, int numberOfReplica = 1, int numberOfShards = 5)
        {
            if (!IsIndexExist(indexName).Exists)
            {
                return _elasticClient.Indices.Create(indexName,
                        c => c.Map<T>(m => m.AutoMap())
                              .Index(indexName)
                              .Settings(a => a.NumberOfReplicas(numberOfReplica)
                              .NumberOfShards(numberOfShards))
                );
            }
            else return null;
        }

        public BulkResponse UpsertDocument(string indexName, List<T> documents, bool autoCreateIndex = true, int numberOfReplica = 1, int numberOfShards = 5)
        {
            if (autoCreateIndex) CreateIndex(indexName, numberOfReplica, numberOfShards);

            return _elasticClient.Bulk(b => b
                   .UpdateMany(documents, (bc, d) => bc
                       .Index(indexName)
                       .Doc(d)
                       .DocAsUpsert(true)
                   ).Refresh(Refresh.True)
                );
        }
        public IndexResponse UpsertDocument(string indexName, T document, bool autoCreateIndex = true)
        {
            if (autoCreateIndex) CreateIndex(indexName);

            return _elasticClient.IndexDocument(document);
        }

        public DeleteResponse DeleteDocument(string index, int id)
        {
            return _elasticClient.Delete<T>(id, bc => bc.Index(index));
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
           // Wrap your existing pre-filter queries in a BoolQuery with Filter
            QueryContainer optimizedQuery = new BoolQuery
            {
                Filter = new List<QueryContainer>
                {
                    elasticSearchBuilder.queryPreFilter
                }
            };


            ISearchRequest<T> searchRequest = new SearchRequest<T>(elasticSearchBuilder.IndexName)
            {
                From = (elasticSearchBuilder.PSF.CurrentPage - 1) * elasticSearchBuilder.PSF.ItemPerPage,
                Size = elasticSearchBuilder.PSF.ItemPerPage,               
                
                Query = optimizedQuery,

                PostFilter = elasticSearchBuilder.queryPostFilter,

                Aggregations = elasticSearchBuilder.aggregationDictionary
            };

            if (elasticSearchBuilder.Source != null)
                searchRequest.Source = new SourceFilter
                {
                    Includes = elasticSearchBuilder.Source.ToArray()
                };



            if (elasticSearchBuilder.Sort != null)
                searchRequest.Sort = elasticSearchBuilder.Sort;

            if (elasticSearchBuilder.TrackTotalHits == false)
                searchRequest.TrackTotalHits = false;

            return _elasticClient.Search<T>(searchRequest);
        }
    }
}