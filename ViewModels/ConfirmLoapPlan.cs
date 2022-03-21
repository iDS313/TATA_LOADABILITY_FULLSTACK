using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.ViewModels
{
    public class ConfirmLoapPlan
    {
        public PlanReq payload { get; set; }
        public List<Priority> priorities { get; set; }
    }
}