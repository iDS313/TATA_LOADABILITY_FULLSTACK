using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class PrDetails
    {
        public int PrDetailsId { get; set; }
        public int CfaId { get; set; }
        public int SkuId { get; set; }
        public Cfa Cfa { get; set; }
        public Sku Sku { get; set; }
        public decimal PrQty { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PrDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}