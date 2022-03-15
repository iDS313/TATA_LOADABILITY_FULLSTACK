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
       
        public ActionResult getLoadPlan(PlanReq pl)
        {
            var truck = _ctx.Trucks.Find(pl.TruckId);
            var priorities = _ctx.Priority.Where(x => x.CfaId == pl.CfaId && x.PlanDate == pl.Plandate.Date && x.Rank > 0).OrderBy(x => x.Rank).ToList();
            var stocks = _ctx.StockDetails.Where(x => x.RecordedAt == pl.Plandate.Date).ToList();
            var unplaned = priorities.Where(x => x.IsPlaned == false && x.FinalQty > 0).OrderBy(x => x.Rank);
            var planed = priorities.Where(x => x.IsPlaned == true).OrderBy(x=>x.Rank);
            List<LoadPlan> loadplan = new List<LoadPlan>();
            var pp = new List<Priority>();
            foreach(var i in unplaned)
            {
                i.Cfa = _ctx.Cfa.Find(i.CfaId);
                i.Sku=_ctx.Sku.Find(i.SkuId);
                if(loadplan.Sum(x=>x.FinalQty)<truck.Limit && loadplan.Sum(x => x.FinalQty)+i.FinalQty < truck.Capacity)
                {
                    var plan = new LoadPlan();
                    plan.FinalQty = i.FinalQty;
                    plan.PlanDate = i.PlanDate;
                    i.IsPlaned = true;
                    plan.CfaId = i.CfaId;
                    plan.SkuId = i.SkuId;
                    var st = stocks.Where(x => x.SkuId == i.SkuId).FirstOrDefault();
                    if (st != null)
                    {
                        st.AvailableQty = st.AvailableQty - plan.FinalQty;
                    }
                    plan.TruckId = truck.TruckId;
                    loadplan.Add(plan);
                    pp.Add(i);
                }
            }
            _ctx.LoadPlans.AddRange(loadplan);
            _ctx.SaveChanges();
            return Json(new { priority = pp,loadplan= loadplan }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult getWeight(PlanReq pl)
        {
            var priorities = _ctx.Priority.Where(x => x.CfaId == pl.CfaId && x.PlanDate == pl.Plandate.Date && x.Rank > 0).OrderBy(x => x.Rank).ToList();
            var stocks = _ctx.StockDetails.Where(x => x.RecordedAt == pl.Plandate.Date).ToList();
            foreach (var pr in priorities)
            {
                pr.Cfa = _ctx.Cfa.Find(pr.CfaId);
                pr.Sku = _ctx.Sku.Find(pr.SkuId);
                var prd = _ctx.PrDetails.Where(x => x.CfaId == pr.CfaId && x.SkuId == pr.SkuId && x.PrDate == pl.Plandate.Date).FirstOrDefault();
                if (prd != null)
                {
                    pr.InPr = true;
                    pr.QtyFromPr = pr.SHQ > prd.PrQty ? prd.PrQty : pr.SHQ;
                    var stock = stocks.Where(x => x.SkuId == pr.SkuId && x.RecordedAt == pr.PlanDate).FirstOrDefault();

                    if (stock != null)
                    {
                        pr.InStock = true;
                        pr.FinalQty = pr.QtyFromPr > stock.AvailableQty ? stock.AvailableQty : pr.QtyFromPr;
                       
                    }
                    else
                    {
                        pr.InStock = false;
                    }
                }
                else
                {
                    pr.InPr = false;
                }

            }
            _ctx.SaveChanges();

            var weight = priorities.Sum(x => x.FinalQty);
            var unplaned = priorities.Where(x => x.IsPlaned == false).OrderBy(x=>x.Rank);
            var planed = priorities.Where(x => x.IsPlaned == true).OrderBy(x=>x.Rank);
            var confirmed = new List<LoadPlan>();
            foreach(var i in planed)
            {
                var c = _ctx.LoadPlans.Where(x => x.CfaId == i.CfaId && x.SkuId == i.SkuId && x.PlanDate == i.PlanDate).FirstOrDefault();
                if (c != null)
                {
                    c.Truck = _ctx.Trucks.Find(c.TruckId);
                    confirmed.Add(c);
                }
            }
            return Json(new { planed = planed, unplaned = unplaned, confirmed = confirmed, weight = weight }, JsonRequestBehavior.AllowGet);
        }
    
    }
}

