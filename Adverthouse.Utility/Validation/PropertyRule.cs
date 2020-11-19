using Adverthouse.Utility.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation
{
    public class PropertyRule
    {
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }

        public List<IPropertyValidator> PropertyValidator { get; set; }
        public PropertyRule(string name, Type type)
        {
            PropertyName = name;
            PropertyType = type;
            PropertyValidator = new List<IPropertyValidator>();
        }
    }
}
