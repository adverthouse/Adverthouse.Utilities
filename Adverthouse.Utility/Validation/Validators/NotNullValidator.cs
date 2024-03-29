﻿namespace Adverthouse.Utility.Validation.Validators
{
    public class NotNullValidator : IPropertyValidator
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage == null ? $"{ProperyName} cannot be null." : _errorMessage;
            }
        }
        public string ScriptRule => $" required : true";
        public string ScriptMessage => $" required :\"{ErrorMessage.Replace("\"", "'")}\"";

        public string ProperyName { get; }
        public bool IsValid(object value)
        {
            var isValid = true;
            if (value == null)
            {
                isValid = false;
            }
            return isValid;
        }
        public NotNullValidator(string propertName)
        {
            ProperyName = propertName;
        }
        public NotNullValidator(string propertName, string errorMessage)
        {
            ProperyName = propertName;
            _errorMessage = errorMessage;
        }
    }
}
