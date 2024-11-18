using Adverthouse.Common.Interfaces;
using Nest;
using System;
using System.Collections.Generic;

namespace Adverthouse.Common.Data.ElasticSearch
{
    public class ElasticSearchBuilder
    {
        public string IndexName { get; private set; }
        public IPSFBase PSF { get; private set; }
        public List<ISort> Sort { get; set; }
        public List<string> Fields { get; set; }

        public QueryContainer queryPreFilter;
        public QueryContainer queryPostFilter;
        public AggregationDictionary aggregationDictionary;
 
        
        private string NormalizeString(string str) { 
           if (String.IsNullOrWhiteSpace(str)) return str;
           string temp = str;
           string[] escapeChars = {"+", "-", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "\"", "~", "*", "?", ":", @"\", "/"};
           temp = temp.Replace(@"\\", "");

           foreach (var escapeChar in escapeChars)
           {
              if (temp.Contains(escapeChar))
                  temp = temp.Replace(escapeChar, @"\\" + escapeChar);
           }
            return temp;
        }

        public ElasticSearchBuilder(string indexName, IPSFBase psf)
        {
            queryPreFilter = new QueryContainer();
            queryPostFilter = new QueryContainer();
            PSF = psf;
            aggregationDictionary = new AggregationDictionary();

            IndexName = indexName;
        }
        public void Prefilter_AddMust<TValue>(string field, TValue value)
        {
            if (value != null)
            {
                queryPreFilter &= new TermQuery()
                {
                    Field = field,
                    Value = value.GetType() == typeof(string) ? NormalizeString(value.ToString()) : value
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
                    Value = value.GetType() == typeof(string) ? NormalizeString(value.ToString()) : value
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
                    Query = NormalizeString(query)
                };
            }
        }
        
        public void Aggregations_AddTerms(string aggregationKey,string field,int size){
            if (!String.IsNullOrWhiteSpace(field))
            {
                aggregationDictionary.Add(aggregationKey,new AggregationContainer(){
                    Terms = new TermsAggregation(field){
                        Field = field,  
                        Size = size
                    }
                });            
            }
        }
    }
}
