using ApiLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Base
{
    public class BaseEntity
    {
        [Omitted]
        public DateTime createdAt { get; set; }
        [Omitted]
        public DateTime? updatedAt { get; set; }
        [Omitted]
        public DateTime? deletedAt { get; set; }


    }
}
