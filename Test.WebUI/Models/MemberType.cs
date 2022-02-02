﻿using Adverthouse.Core;
using System;

namespace Test.WebUI.Models
{
    public enum MemberType
    {
        [Enum("Supplier","success")]
        supplier = 0,
        [Enum("Member", "danger")]
        member =1,
        [Enum("Home user", "info")]
        homeUser =2
    }
}
