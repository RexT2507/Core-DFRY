using ApiLibrary.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Base
{
    public class BaseModel<T> : BaseEntity
    {
        [Omitted]
        public T ID { get; set; }
    }
}
