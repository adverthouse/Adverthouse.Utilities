using System.Collections.Generic;

namespace Adverthouse.Common.Data
{
    public class PagedList<T, TPSF>
    {
        public IEnumerable<T> Data { get; set; }
        public TPSF PSF { get; set; }
                
        public PagedList() { }
        public PagedList(TPSF psf, IEnumerable<T> data)
        {
            PSF = psf;
            Data = data;
        } 
    }
}
