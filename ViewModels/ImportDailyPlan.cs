using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loadability.ViewModels
{
    public class ImportDailyPlan
    {
        [Display(Name="CFA")]
        public string  CFA { get; set; }
        [Display(Name = "SKU")]
        public string SKU { get; set; }
        [Display(Name = "Priority")]
        public decimal Priority { get; set; }
        [Display(Name = "SIT")]
        public decimal SIT { get; set; }
       // [Display(Name = "PlanDate")]
       // public DateTime PlanDate { get; set; } = DateTime.Now.Date;
    }
}