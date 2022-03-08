using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class Priority
    {
        public int PriorityId { get; set; }
        public int SkuId { get; set; }
        public int CfaId { get; set; }
        public Cfa Cfa { get; set; }
        public Sku Sku { get; set; }
        public decimal Qty { get; set; }
        public int Level { get; set; }
        public DateTime Scheduled { get; set; }
    }
}