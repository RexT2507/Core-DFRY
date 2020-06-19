using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class OmittedAttribute : Attribute
    {
        public OmittedAttribute()
        {

        }
    }
}
