using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data
{
    public class ListingResult<T, PSF>
    {
        public IEnumerable<T> Data { get; set; }
        public PSF PagingInfo { get; set; }              
        public ListingResult() { }
        public ListingResult(PSF pagingInfo, IEnumerable<T> data) 
        {
            PagingInfo = pagingInfo;
            Data = data;
        }
    }
}
