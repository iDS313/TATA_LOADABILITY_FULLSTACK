using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class StockDetails
    {
        public int StockDetailsId { get; set; }
        public int SkuId { get; set; }
        public Sku Sku { get; set; }
        public decimal AvailableQty  { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}