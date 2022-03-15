using Loadability.Models;
using Loadability.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loadability.Controllers
{
    public class LoadPlanController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public LoadPlanController()
        {
            _ctx = new ApplicationDbContext();
        }
        // GET: LoadPlan
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult getLoadPlan(PlanReq pl)
        //{


        //}

        [HttpPost]
        public ActionResult getWeight(PlanReq pl)
        {
            var priorities = _ctx.Priority.Where(x => x.CfaId == pl.CfaId && x.PlanDate == pl.Plandate.Date && x.Rank>0).OrderBy(x=>x.Rank).ToList();
            var weight = 0;
            foreach(var i in priorities)
            {
                var pr = _ctx.PrDetails.Where(x => x.CfaId == i.CfaId && x.SkuId == i.SkuId).FirstOrDefault();
                if(pr != null)
                {

                }
            }
        }

    }
}

