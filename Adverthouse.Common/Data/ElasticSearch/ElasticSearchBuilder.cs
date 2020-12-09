using Adverthouse.Common.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data.ElasticSearch
{
    public class ElasticSearchBuilder
    {
        public string IndexName { get; private set; }
        public IPSFBase PSF { get; private set; }
        public List<ISort> Sort { get; set; }

        public QueryContainer queryPreFilter;
        public QueryContainer queryPostFilter;

        public ElasticSearchBuilder(string indexName, IPSFBase psf)
        {
            queryPreFilter = new QueryContainer();
            queryPostFilter = new QueryContainer();
            PSF = psf;

            IndexName = indexName;
        }
        public void Prefilter_AddMust<TValue>(string field, TValue value)
        {
            if (value != null)
            {
                queryPreFilter &= new TermQuery()
                {
                    Field = field,
                    Value = value
                };
            }
        }
        public void Prefilter_AddMustNot<TValue>(string field, TValue value) 
        {
            if (value != null)
            {
                queryPreFilter = !new TermQuery()
                {
                    Field = field,
                    Value = value
                };
            }
        }

        public void Prefilter_AddMustGreaterEqual(string field, double? value)
        {
            if (value != null)
            {
                queryPreFilter &= new NumericRangeQuery()
                {
                    Field = field,
                    GreaterThanOrEqualTo = value
                };
            }
        }
        public void Prefilter_AddMustQuery(string fields, string query)
        {
            if (!String.IsNullOrWhiteSpace(query))
            {
                queryPreFilter &= new QueryStringQuery()
                {
                    Fields = fields,
                    Query = query
                };
            }
        }

    }
}
