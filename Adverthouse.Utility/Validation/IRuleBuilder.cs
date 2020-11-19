using Adverthouse.Utility.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation
{
    public interface IRuleBuilder
    {
        PropertyRule ValidationRule { get; }
        void AddRule(IPropertyValidator validator);
    }
}
