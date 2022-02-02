using System;

namespace Adverthouse.Utility.Validation.Validators
{
    public class RequiredValidator : IPropertyValidator
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage == null ? $"{ProperyName} required" : _errorMessage;
            }
        }
        public string ScriptRule => $" required : true"; 
        public string ScriptMessage => $" required :\"{ErrorMessage.Replace("\"", "'")}\""; 
        public string ProperyName { get; }
        public bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return false;
            }
            var isValid = true;
            if (value.ToString().Trim() == string.Empty)
            {
                isValid = false;
            }
            return isValid;
        }
        public RequiredValidator(string propertName)
        {
            ProperyName = propertName;
        }
        public RequiredValidator(string propertName, string errorMessage)
        {
            ProperyName = propertName;
            _errorMessage = errorMessage;
        }
    }
}
