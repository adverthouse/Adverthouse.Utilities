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

        public PermissionType GetPermissionTypeByDescription(string description)
        {
            var TPT = PermissionType.BaseAccess;
            switch (description)
            {
                case "PT1":
                    TPT = PermissionType.BaseAccess;
                    break;
                case "PT2":
                    TPT = PermissionType.Insert;
                    break;
                case "PT3":
                    TPT = PermissionType.Update;
                    break;
                case "PT4":
                    TPT = PermissionType.Delete;
                    break;
                case "PT5":
                    TPT = PermissionType.Extra1;
                    break;
                case "PT6":
                    TPT = PermissionType.Extra2;
                    break;
                case "PT7":
                    TPT = PermissionType.Extra3;
                    break;
                case "PT8":
                    TPT = PermissionType.Extra4;
                    break;
            }
            return TPT;
        }
    }
}
