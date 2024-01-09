using System;

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
        private object CompareValue { get;set;}
        public string ProperyName { get; }
        public string ComparePropertyName { get; }

        public string ScriptRule => $" equalTo : '#{ComparePropertyName}'"; 

        public string ScriptMessage => $" equalTo :\"{ErrorMessage.Replace("\"", "'")}\"";

        public bool IsValid(object value) 
        {
            if (value.ToString() == CompareValue.ToString())
               return true;
            else return false;
        } 

        public CompareValidator(string propertName, string comparePropertyName)
        {
            ProperyName = propertName;
            ComparePropertyName = comparePropertyName;
        }
        public CompareValidator(string propertName, string comparePropertyName,object compareValue, string errorMessage)
        {
            ProperyName = propertName;
            ComparePropertyName = comparePropertyName; 
            CompareValue = compareValue;
            _errorMessage = errorMessage;
        }
    }
}
