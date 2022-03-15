using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class DailyPlan
    {
        public int DailyPlanId { get; set; }
        public int CfaId { get; set; }
        public int SkuId { get; set; }
        [ForeignKey("CfaId")]
        public Cfa Cfa { get; set; }
        [ForeignKey("SkuId")]
        public Sku Sku { get; set; }
        public decimal PriorityQty { get; set; }

        public decimal SIT { get; set; }
        public decimal SHQ { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime PlanDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        public string  CreatedBy { get; set; }
    }
}