using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.ViewModels
{
    public class DailyPlanDto
    {
        public HttpPostedFileBase excel { get; set; }
        public string  SaveDate { get; set; }
    }
}