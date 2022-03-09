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
        public int DailyPlanId { get; set; }
        public decimal Qty { get; set; }
        [ForeignKey("DailyPlanId")]
        public DailyPlan DailyPlan { get; set; }
        public int CfaId { get; set; }
        [ForeignKey("CfaId")]
        public Cfa Cfa { get; set; }
        public int Rank { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Scheduled { get; set; }
    }
}