using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.ViewModels
{
    public class PriorityDto
    {
        public int Rank { get; set; }
        public string  Cfa { get; set; }
        public string  Sku { get; set; }
        public decimal PriorityQty { get; set; }
        public decimal SHQ { get; set; }
    }
}