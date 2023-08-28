using Loadability.Models;
using Loadability.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ActionResult PlanedPriorities()
        {
            return View();
        }

        //public ActionResult getLoadPlan(PlanReq pl)
        //{


        //}
        public ActionResult getPriorities(PlanReq pl)
        {
            var priorities = _ctx.Priority.Include(x => x.Cfa).Include(x => x.Sku).Where(x => x.CfaId == pl.CfaId && x.PlanDate == pl.Plandate.Date && x.Rank > 0).OrderBy(x => x.Rank).ToList();
            var planed = priorities.Where(x => x.IsLoaded == true).OrderBy(x=>x.Rank);
            var unplaned = priorities.Where(x => x.IsLoaded == false || (x.IsLoaded == true && x.PendingQty > 0)).OrderBy(x => x.Rank);
            var loadplan =new List<LoadPlan>();
            foreach(var v in planed)
            {
                var lp = _ctx.LoadPlans.Include(x=>x.Truck).Include(x=>x.Cfa).Include(x=>x.Sku).Where(x => x.CfaId == v.CfaId && x.SkuId == v.SkuId && x.PlanDate == v.PlanDate);
                loadplan.AddRange(lp);
            }
            return Json(new { planed = planed, unplaned = unplaned , loadplan=loadplan}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult getInitialData(PlanReq pl)
        {
            var priorities = _ctx.Priority.Where(x => x.CfaId == pl.CfaId && x.PlanDate == pl.Plandate.Date && x.Rank>0).OrderBy(x=>x.Rank).ToList();
            foreach(var i in priorities)
            {
                i.Cfa = _ctx.Cfa.Find(i.CfaId);
                i.Sku = _ctx.Sku.Find(i.SkuId);
                var pr = _ctx.PrDetails.Where(x => x.CfaId == i.CfaId && x.SkuId == i.SkuId && (x.IsPlaned==0|| x.IsPlaned==2)).FirstOrDefault();
                if ( i.IsCompared == false)
                {
                    if (pr != null)
                    {
                        i.InPr = true;
                        i.QtyFromPr = i.SHQ > pr.PrQty ? pr.PrQty : i.SHQ;
                        pr.SuppliedQty = pr.SuppliedQty + pr.PrQty - i.QtyFromPr;
                        if (pr.PrQty - pr.SuppliedQty > 0)
                        {
                            pr.IsPlaned = 2;
                        }
                        else if (pr.PrQty - pr.SuppliedQty == 0)
                        {
                            pr.IsPlaned = 1;
                        }
                        else
                        {
                            pr.IsPlaned = 0;
                        }
                        _ctx.SaveChanges();
                    }
                    else
                    {
                        i.InPr = false;
                    }

                    var stk = _ctx.StockDetails.Where(x => x.SkuId == i.SkuId).FirstOrDefault();
                    if (stk != null && stk.AvailableQty > 0)
                    {
                        i.InStock = true;
                        if (i.FinalQty < 1)
                        {
                            i.FinalQty = i.QtyFromPr > stk.AvailableQty ? stk.AvailableQty : i.QtyFromPr;
                            stk.AvailableQty = stk.AvailableQty - i.FinalQty;
                            _ctx.SaveChanges();
                        }
                        else
                        {
                            stk.AvailableQty = stk.AvailableQty + i.FinalQty;
                            i.FinalQty = i.QtyFromPr > stk.AvailableQty ? stk.AvailableQty : i.QtyFromPr;
                            stk.AvailableQty = stk.AvailableQty - i.FinalQty;
                            _ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        i.InStock = false;
                    }
                    if (i.LoadedQty < 1)
                    {
                        i.PendingQty = i.FinalQty;
                    }
                    i.IsCompared = true;
                }
            }
            _ctx.SaveChanges();
            var lp = new List<LoadPlan>();
            var planed = priorities.Where(x => x.IsLoaded == true).OrderBy(c => c.Rank);
            foreach (var c in planed)
            {
                var load = _ctx.LoadPlans.Include(x=>x.Truck).Include(x=>x.Cfa).Include(y=>y.Sku).Where(x => x.CfaId == c.CfaId && x.SkuId == c.SkuId && x.PlanDate == c.PlanDate);
                if (load != null)
                {
                    lp.AddRange(load);
                }
            }
            var weight = priorities.Where(x=>x.InStock==true && x.InPr==true ).Sum(x => x.FinalQty);
            var remain = priorities.Where(x => (x.IsLoaded == false ||  (x.IsLoaded == true &&  x.PendingQty>0) ) && x.Rank > 0).Sum(x => x.PendingQty);
            return Json(new { weight = weight, priorities = priorities,remaining=remain, loadplan=lp }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPlan(PlanReq pl)
        {
            var priorities = _ctx.Priority.AsNoTracking().Include(x=>x.Sku).Include(x=>x.Cfa).Where(x => x.CfaId == pl.CfaId && x.PlanDate == pl.Plandate.Date && x.Rank > 0 ).OrderBy(x => x.Rank).ToList();
            var unplaned = priorities.Where(x => x.IsLoaded == false || (x.IsLoaded==true && x.PendingQty>0) ).OrderBy(x => x.Rank);
            var planed = priorities.Where(x => x.IsLoaded == true).OrderBy(c => c.Rank);
            var truck = _ctx.Trucks.Find(pl.TruckId);
            List<Priority> p = new List<Priority>();
            var lp = new List<LoadPlan>();
            //var fdiff = 0.00;

            foreach(var v in unplaned)
            {
                var sum = p.Sum(x => x.ToLoad) ;
                if (sum >= truck.Capacity)
                {
                    break;
                }
                if (sum< truck.Capacity || sum < truck.Limit )
                {
                    if ((sum + v.PendingQty < truck.Limit|| sum + v.PendingQty < truck.Capacity))
                    {
                        v.ToLoad = v.PendingQty;
                        v.LoadedQty = v.LoadedQty + v.PendingQty;
                        v.PendingQty = 0;
                        p.Add(v);
                        
                    }
                    else
                    {
                      //  var w = _ctx.Priority.AsNoTracking().Include(x => x.Sku).Include(x => x.Cfa).FirstOrDefault(x => x.PriorityId == v.PriorityId);
                        v.ToLoad = (truck.Capacity - sum);
                        v.LoadedQty = v.LoadedQty + (truck.Capacity - sum);
                        v.PendingQty = v.FinalQty - v.LoadedQty;
                        p.Add(v);
                    }
                   
                }
               
            }
            foreach(var c in planed)
            {
                var load = _ctx.LoadPlans.Include(x=>x.Truck).Include(e=>e.Sku).Include(d=>d.Cfa).Where(x => x.CfaId == c.CfaId && x.SkuId == c.SkuId && x.PlanDate == c.PlanDate);
                if (load != null)
                {
                    lp.AddRange(load);
                }
            }
            var g = unplaned.Sum(x => x.PendingQty);
            //  var k = p.Sum(x => x.ToLoad);
            var remain = g;// - k;
            return Json(new { planed = p, loadplan = lp, remaining = remain },JsonRequestBehavior.AllowGet);

        }
        public ActionResult Confirm(ConfirmLoapPlan confirm) 
        {
            var truck = _ctx.Trucks.Find(confirm.payload.TruckId);
          
            List<Priority> pr = new List<Priority>();
            var lp = new List<LoadPlan>();
            foreach (var p in confirm.priorities)
            {
                var prio = _ctx.Priority.Where(x => x.CfaId == p.CfaId && x.SkuId == p.SkuId && x.PlanDate == p.PlanDate.Date).FirstOrDefault();
                var load = new LoadPlan();
                load.CfaId = prio.CfaId;
                load.SkuId = prio.SkuId;
                load.PlanDate = prio.PlanDate;
                load.Qty = p.ToLoad;
                prio.LoadedQty = prio.LoadedQty + p.ToLoad;
                prio.PendingQty = prio.FinalQty - prio.LoadedQty;
                
                load.TruckId = truck.TruckId;
                prio.IsLoaded = true;
                _ctx.LoadPlans.Add(load);
                _ctx.SaveChanges();
                
            }
            var prioritiez = _ctx.Priority.Where(x => x.PlanDate == confirm.payload.Plandate.Date && x.CfaId == confirm.payload.CfaId && x.Rank > 0).OrderBy(x => x.Rank).ToList();
            var unplaned = prioritiez.Where(x => x.IsLoaded == false || (x.IsLoaded==true  )).OrderBy(x => x.Rank);
            var planed = prioritiez.Where(x => x.IsLoaded == true).OrderBy(c => c.Rank);
            foreach (var c in planed)
            {
                var load = _ctx.LoadPlans.Include(e => e.Sku).Include(x=>x.Truck).Include(d => d.Cfa).Where(x => x.CfaId == c.CfaId && x.SkuId == c.SkuId && x.PlanDate == c.PlanDate);
                if (load != null)
                {
                    lp.AddRange(load);
                }
            }
            var remain = unplaned.Sum(x => x.PendingQty) - pr.Sum(x => x.LoadedQty);
            return Json(new { loadplan = lp, remaining = remain }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getplanFromPr(DateTime d, decimal cap, int CfaId)
        {
            var todayDate = new DateTime(d.Year, d.Month, d.Day);
            d = d.AddDays(1).Date;
            var stk = _ctx.StockDetails.Where(x => x.AvailableQty > 1000 && x.RecordedAt == todayDate).OrderByDescending(x=>x.AvailableQty).ToList();
            var priorities = new List<Priority>();
            var i = 0;
            while (priorities.Sum(x => x.PendingQty) < cap &&  i< stk.Count)
            {
                var abc = stk[i];
                var m = new Priority();
                var pr = _ctx.PrDetails.Where(x => x.CfaId == CfaId && x.PrDate == d && x.SkuId == abc.SkuId).FirstOrDefault();
                i++;
                if (pr == null)
                    continue;
                m.Cfa = _ctx.Cfa.Where(s => s.CfaId == CfaId).FirstOrDefault();
               
                m.SkuId = stk[i].SkuId;
                m.Sku = _ctx.Sku.Where(s => s.SkuId == m.SkuId).FirstOrDefault();
                m.CfaId = CfaId;
                m.PlanDate = pr.PrDate;
                m.InPr = true;
                m.InStock = true;
                if (priorities.Sum(x => x.PendingQty)+ pr.PrQty<  cap)
                {
                    m.FinalQty = pr.PrQty;
                }
                else
                {
                    m.FinalQty= cap-priorities.Sum(x => x.PendingQty);
                    //  = cap - pr.PrQty;
                }
                m.PendingQty = m.FinalQty;
                m.ToLoad = m.PendingQty;
                m.LoadedQty = 0;
                priorities.Add(m);
            }
            // d.AddDays(1);
            return Json(new { planed = priorities }, JsonRequestBehavior.AllowGet);
                
        }
        
    }
}

