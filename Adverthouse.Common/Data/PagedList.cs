using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;

namespace Adverthouse.Common.Data
{
    public class PagedList<T, PSF>
    {
        public IEnumerable<T> Data { get; set; }
        public PSF PagingInfo { get; set; }
        public PagedList() { }
        public PagedList(PSF pagingInfo, IEnumerable<T> data)
        {
            PagingInfo = pagingInfo;
            Data = data;
        } 
    }
}
