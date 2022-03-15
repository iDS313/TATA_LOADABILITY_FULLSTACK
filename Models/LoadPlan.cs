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
        public int TruckId { get; set; }
       
        public int CfaId { get; set; }
        public Cfa Cfa { get; set; }
        public Sku Sku { get; set; }
        public decimal FinalQty { get; set; }
        public decimal LoadedQty { get; set; }
        public decimal PendingQty { get; set; }

        public bool IsLoaded { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PlanDate { get; set; }
        [ForeignKey("TruckId")]
        public Truck Truck { get; set; }
        public StockDetails StockDetails { get; set; }
       
       
    }
}