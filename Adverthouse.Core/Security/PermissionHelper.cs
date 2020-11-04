using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Security
{
    [Serializable]
    public sealed class PermissionHelper
    {
        public PermissionHelper() { }
        public PermissionHelper(string section, PermissionType typeOfPermission)
        {
            Section = section;
            TypeOfPermission = typeOfPermission;
        }
        public string Section { get; set; }

        public PermissionType TypeOfPermission { get; set; }
    }
}
