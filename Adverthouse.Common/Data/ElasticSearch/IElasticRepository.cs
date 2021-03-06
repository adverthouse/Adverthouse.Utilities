﻿using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data.ElasticSearch
{
    public interface IElasticRepository<T> where T : class
    {
        ExistsResponse IsIndexExist(string indexName);
        CreateIndexResponse CreateIndex(string indexName, int numberOfReplica = 1, int numberOfShards = 5);
        DeleteIndexResponse DeleteIndex(string indexName);
        BulkResponse UpsertDocument(string indexName, List<T> documents, bool autoCreateIndex = true);
        ISearchResponse<T> Search(ElasticSearchBuilder elasticSearchBuilder);
    }
}
