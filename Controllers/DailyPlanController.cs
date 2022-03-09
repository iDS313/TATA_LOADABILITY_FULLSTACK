using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Loadability.ViewModels;
using MiniExcelLibs;

namespace Loadability.Controllers
{
    public class DailyPlanController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public DailyPlanController()
        {
            _ctx = new ApplicationDbContext();
        }
        // GET: LoadPlan
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getPlan(DailyPlan lp)
        {
            lp.PlanDate = lp.PlanDate.Date;
            var p = _ctx.DailyPlan.AsNoTracking().Where(x => x.CfaId == lp.CfaId && x.SkuId == lp.SkuId && x.PlanDate == lp.PlanDate).FirstOrDefault();
            if (p != null)
            {
                return Json(p, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Response.StatusCode = 404;
                return Json(lp, JsonRequestBehavior.AllowGet);
            }
            //return View();
        }
        public ActionResult Save(DailyPlan lp)
        {
            lp.PlanDate = lp.PlanDate.Date;
            var p = _ctx.DailyPlan.AsNoTracking().Where(x => x.CfaId == lp.CfaId && x.SkuId == lp.SkuId && x.PlanDate == lp.PlanDate).FirstOrDefault();
            if (p == null)
            {
                _ctx.DailyPlan.Add(lp);
            }
            else
            {
                _ctx.Entry(lp).State = System.Data.Entity.EntityState.Modified;
            }
            _ctx.SaveChanges();
            return Json(lp,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase excel, DateTime? date)
        {
            try
            {
                if (excel.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(excel.FileName);
                    if (!Directory.Exists(Server.MapPath("~/Content/Excel/DailyPlan_" + DateTime.UtcNow.Month)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Excel/DailyPlan_" + DateTime.UtcNow.Month));
                    }
                    string _path = Path.Combine(Server.MapPath("~/Content/Excel/DailyPlan_" + DateTime.UtcNow.Month),"Plan_"+DateTime.UtcNow.ToString("dd")+"_"+ _FileName);
                    excel.SaveAs(_path);
                   
                   var Data=MiniExcel.Query<ImportDailyPlan>(_path);
                    List<DailyPlan> dp= new List<DailyPlan>();

                    foreach (var a in Data)
                    {
                        var cfaid = _ctx.Cfa.Where(x => x.DepoCode == a.CFA).FirstOrDefault().CfaId;
                        var skuid = _ctx.Sku.Where(x => x.SkuCode == a.SKU).FirstOrDefault().SkuId;
                        var plan = _ctx.DailyPlan.Where(x => x.CfaId == cfaid && x.SkuId == skuid && x.PlanDate == a.PlanDate.Date).FirstOrDefault();
                        if (plan != null)
                        {
                            plan.PriorityQty = a.Priority;
                            plan.QtyInTransit = a.SIT;
                            _ctx.SaveChanges();
                            Prioritize(plan.CfaId,plan.PlanDate);
                        }
                        else
                        {
                           var ndp=  new DailyPlan
                            {
                                CfaId = cfaid,
                                SkuId = skuid,
                                PriorityQty = a.Priority,
                                QtyInTransit = a.SIT,
                                PlanDate = a.PlanDate
                            };
                            _ctx.DailyPlan.Add(ndp);
                            _ctx.SaveChanges();
                            Prioritize(cfaid, a.PlanDate);
                        }
                       
                    }


                    return Json(dp, JsonRequestBehavior.AllowGet);

                }
               // ViewBag.Message = "File Uploaded Successfully!!";
                return Json("File Save Successfully");
            }
            catch(Exception e)
            {
               // ViewBag.Message = "File upload failed!!";
                Response.StatusCode = 400;
                return Json("Unable To Save File, Make sure File is Selected");
            }
           
        }

        private bool Prioritize( int cfa, DateTime PlanDate )
        {
            var dailyplan = _ctx.DailyPlan.Where(x => x.CfaId == cfa && x.PlanDate == PlanDate).OrderByDescending(x => x.PriorityQty).ToList();
            List<Priority> priorities= new List<Priority>();
            foreach (var i in dailyplan)
            {
               var p= _ctx.Priority.Where(x => x.DailyPlanId == i.DailyPlanId).FirstOrDefault();
                if (p == null)
                {
                    priorities.Add(new Priority
                    {
                        DailyPlanId = i.DailyPlanId,
                        Qty = i.PriorityQty - i.QtyInTransit,
                        Scheduled = PlanDate,
                        CfaId=i.CfaId,
                    }); ;
                }
                else
                {
                    p.Qty = i.PriorityQty - i.QtyInTransit;
                    p.Scheduled = PlanDate;
                    p.CfaId = i.CfaId;
                    _ctx.SaveChanges();
                }
            }
            _ctx.Priority.AddRange(priorities);
            _ctx.SaveChanges();
            var priority = _ctx.Priority.Where(x => x.CfaId == cfa && x.Scheduled == PlanDate && x.Qty>0).OrderByDescending(x => x.Qty).ToList();
            var j = 1;
            foreach(var i in priority)
            {
                i.Rank = j;
                j++;
            }
            _ctx.SaveChanges();
            return true;

        }
      
    }
}