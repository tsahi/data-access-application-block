﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.Accessor.TestObjects
{
    public class BaseClassWithGenericProperty<T>
    {
        public T Something { get; set; }
    }
}