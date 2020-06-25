using ApiLibrary.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Cocktails.API.Models;

namespace Orders.API.Models
{
    public class OrderDetails : BaseModel<int>
    {
        public int? ID_Order { get; set; }
        [ForeignKey("ID_Order")]
        public Order order { get; set; }
        public int? ID_Cocktail { get; set; }
        [ForeignKey("ID_Cocktail")]
        public Cocktail cocktail { get; set; }
        public int Quantite { get; set; }


    }
}
