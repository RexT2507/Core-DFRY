using ApiLibrary.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cocktails.API.Models
{
    public class Ingredient : BaseModel<int>
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Nom { get; set; }
        [Column(TypeName = "decimal(2,2)")]
        public decimal Teneur_Alcool { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Mesure { get; set; }
    }
}
