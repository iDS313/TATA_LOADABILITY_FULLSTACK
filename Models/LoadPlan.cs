using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class LoadPlan
    {
        public int LoadPlanId { get; set; }
        public int SkuId { get; set; }
        public int PriorityId { get; set; }
        public int PrDetailsId { get; set; }
        public int StockDetailsId { get; set; }
        public int CfaId { get; set; }
        public Cfa Cfa { get; set; }
        public Sku Sku { get; set; }
        public decimal Qty { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PlanDate { get; set; }
        public StockDetails StockDetails { get; set; }
        public PrDetails PrDetails { get; set; }
        public Priority Priority { get; set; }
    }
}