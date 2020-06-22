using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class MaxPaginationAttribute : Attribute
    {

        public int Range { get; private set; }
        public MaxPaginationAttribute(int maxRange)
        {
            Range = maxRange;
        }
    }
}
