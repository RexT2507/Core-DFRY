using ApiLibrary.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cocktails.API.Models
{
    public class Cocktail : BaseModel<int>
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Nom { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Origine { get; set; }
        public bool Alcool { get; set; }
        [Column(TypeName = "decimal(3,1)")]
        public decimal Rating { get; set; }

    }
}
