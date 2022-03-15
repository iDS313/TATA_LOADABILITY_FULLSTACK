using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class Priority
    {
        public int PriorityId { get; set; }
        public int SkuId { get; set; }
        public decimal SHQ { get; set; }
       
        public int CfaId { get; set; }
        [ForeignKey("CfaId")]
        public Cfa Cfa { get; set; }
        [ForeignKey("SkuId")]
        public Sku Sku { get; set; }
        public int Rank { get; set; }
        public bool InPr { get; set; }
        public bool InStock { get; set; }
        public decimal QtyFromPr { get; set; }
        public decimal FinalQty { get; set; }
        public bool IsPlaned { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PlanDate { get; set; }
    }
}