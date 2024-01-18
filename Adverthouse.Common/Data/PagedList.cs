using System.Collections.Generic;

namespace Adverthouse.Common.Data
{
    public class PagedList<T> : IPagedList<T>
    {
        public IEnumerable<T> Data { get; set; }                
        public PagedList() { }
        public PagedList(IEnumerable<T> data)
        {
            Data = data;
        } 
    }
}
