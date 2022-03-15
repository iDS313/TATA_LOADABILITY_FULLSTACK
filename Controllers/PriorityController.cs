using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loadability.Controllers
{
    public class PriorityController : Controller
    {
        // GET: Priority
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getPriority(int CfaId, DateTime PlanDate)
        {
            var ctx = new ApplicationDbContext();
            var priorities= ctx.Priority.Where(x=>x.CfaId==CfaId && x.PlanDate == PlanDate.Date && x.Rank>0).OrderBy(p=>p.Rank).Select( x=>new
            {
                Cfa=x.Cfa.CfaLocation+ " - "+ x.Cfa.DepoCode,
                Sku =x.Sku.SkuTitle + " - "+ x.Sku.SkuCode,
                Rank= x.Rank,
                PriorityQty=ctx.DailyPlan.Where(s => s.CfaId == CfaId && s.SkuId==x.SkuId && s.PlanDate == PlanDate.Date).Select(t=>t.PriorityQty).FirstOrDefault(),
                SHQ=x.SHQ
            }).ToList();
            if(priorities.Count()>0)
            return Json(priorities, JsonRequestBehavior.AllowGet);
            else
            {
                Response.StatusCode = 404;
                return Json(new { Message = "Not Found" });
            }
        }
    }
}