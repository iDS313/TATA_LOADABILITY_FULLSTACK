using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.ViewModels
{
    public class PlanReq
    {
        public int CfaId { get; set; }
        public int TruckId { get; set; }
        public DateTime Plandate { get; set; }
    }
}