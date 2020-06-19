using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Base
{
    public class BaseModel<T> : BaseEntity
    {
        public T ID { get; set; }
    }
}
