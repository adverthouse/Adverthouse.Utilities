using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation.Validators
{
    public class CheckValidator : IPropertyValidator
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

        public string ScriptRule => $" required : true \r\n";
        public string ScriptMessage => $" required :\"{ErrorMessage.Replace("\"", "'")}\" \r\n";

        public bool IsValid(object value)
        {
            return (Convert.ToString(value).ToLower() == "true");
        }
        public CheckValidator(string propertName)
        {
            ProperyName = propertName;
        }
        public CheckValidator(string propertName, string errorMessage)
        {
            ProperyName = propertName;
            _errorMessage = errorMessage;
        }
    }
}
