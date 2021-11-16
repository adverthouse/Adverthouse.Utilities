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
        
        private string NormalizeString(string str) { 
           if (String.IsNullOrWhiteSpace(str)) return str;
           string temp = str;
           string[] escapeChars = {"+", "-", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "\"", "~", "*", "?", ":", @"\", "/"};
           foreach(var escapeChar in escapeChars)
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

    }
}
