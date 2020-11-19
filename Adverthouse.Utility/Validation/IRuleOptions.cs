using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation
{
    public interface IRuleOptions
    {
        IRuleBuilder NotNull();
        IRuleBuilder GreaterThen();
    }
}
