using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation.Validators
{
    public interface IPropertyValidator
    {
        string Name => GetType().Name;
        string ErrorMessage { get; }
        string ProperyName { get; }
        bool IsValid(object value);
    }
}
