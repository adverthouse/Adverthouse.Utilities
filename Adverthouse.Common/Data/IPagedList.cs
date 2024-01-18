using System.Collections.Generic;

namespace Adverthouse.Common.Data
{
    public interface IPagedList<T>
    {
        IEnumerable<T> Data { get; set; }      
    }
}
