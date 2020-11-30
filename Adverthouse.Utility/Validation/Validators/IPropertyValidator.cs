using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation.Validators
{
    public interface IPropertyValidator
    {
        string ScriptRule { get; }
        string ScriptMessage { get; }
        string Name => GetType().Name;
        string ErrorMessage { get; }
        string ProperyName { get; }
        bool IsValid(object value);
    }
}
