using ApiLibrary.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cocktails.API.Models
{
    public class Preparation : BaseModel<int>
    {
        public int? ID_Cocktail { get; set; }
        [ForeignKey("ID_Cocktail")]
        public Cocktail cocktail { get; set; }
        public int? ID_Ing { get; set; }
        [ForeignKey("ID_Ing")]
        public Ingredient ingredient { get; set; }
        [Column(TypeName = "decimal(3,2)")]
        public decimal Quantite { get; set; }


    }
}
