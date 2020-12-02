using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumAttribute:Attribute
    {
        public static readonly EnumAttribute Default = new EnumAttribute();
        private string _description;
        private string _className;

        public EnumAttribute() : this(string.Empty,string.Empty) { }
        public EnumAttribute(string description,string className)
        {
            _description = description;
            _className = className;
        }

        public virtual string Description => DescriptionValue;  

        protected string DescriptionValue
        {
            get => _description; 
            set => _description = value; 
        }
        public virtual string ClassName => ClassNameValue;

        protected string ClassNameValue
        {
            get => _className;
            set => _className = value;
        }
    }
}
