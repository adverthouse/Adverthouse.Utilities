using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data
{
    public class PSNoFilter : PSFBase
    {
        public override string Filter
        {
            get
            {
                return "";
            }
        }
        public PSNoFilter() { }
    }
}
