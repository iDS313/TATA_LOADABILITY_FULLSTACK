using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class StockDetails
    {
        public int StockDetailsId { get; set; }
        public int SkuId { get; set; }
        public Sku Sku { get; set; }
        public decimal StartQty  { get; set; }
        public decimal AvailableQty  { get; set; }
        public decimal EndingQty  { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime RecordedAt { get; set; }
    }
}