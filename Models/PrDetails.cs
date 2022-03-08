using System;
using System.Collections.Generic;
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
        public DateTime IssueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}