using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class LoadPlan
    {
        public int LoadPlanId { get; set; }
        public Cfa Cfa { get; set; }
        public Sku Sku { get; set; }
        public decimal PriorityQty { get; set; }
        public decimal QtyInTransit { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string  CreatedBy { get; set; }
    }
}