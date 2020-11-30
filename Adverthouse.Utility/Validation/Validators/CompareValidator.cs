using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation.Validators
{
    public class CompareValidator : IPropertyValidator
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage == null ? $"{ProperyName} required" : _errorMessage;
            }
        }
        public string ProperyName { get; }
        public string ComparePropertyName { get; }

        public string ScriptRule => $" equalTo : '#{ComparePropertyName}' \r\n"; 

        public string ScriptMessage => $" equalTo :\"{ErrorMessage.Replace("\"", "'")}\" \r\n";

        public bool IsValid(object value) => false;
        public bool IsValid(object value, object CompareValue)
        {
            return ((Convert.ToString(value) ?? "") == (Convert.ToString(CompareValue) ?? "") ? true : false);
        }
        public CompareValidator(string propertName, string comparePropertyName)
        {
            ProperyName = propertName;
            ComparePropertyName = comparePropertyName;
        }
        public CompareValidator(string propertName, string comparePropertyName, string errorMessage)
        {
            ProperyName = propertName;
            ComparePropertyName = comparePropertyName;
            _errorMessage = errorMessage;
        }
    }
}
