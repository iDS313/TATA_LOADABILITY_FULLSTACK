using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Loadability.ViewModels;
using MiniExcelLibs;
using Loadability.Services;

namespace Loadability.Controllers
{
    public class DailyPlanController : Controller
    {
        protected readonly ApplicationDbContext _ctx;

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
                lp.SHQ = lp.PriorityQty - lp.SIT;
                _ctx.DailyPlan.Add(lp);
            }
            else
            {
                lp.SHQ = lp.PriorityQty - lp.SIT;
                _ctx.Entry(lp).State = System.Data.Entity.EntityState.Modified;
            }
            _ctx.SaveChanges();
            Prioritization.Prioritize(lp.PlanDate,_ctx);
            return Json(lp,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadExcel( DailyPlanDto updp)
        {
            var excel = updp.excel;
            var date=Convert.ToDateTime(updp.SaveDate);
            try
            {
                if (excel.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(excel.FileName);
                    if (!Directory.Exists(Server.MapPath("~/Content/Excel/DailyPlan_" + DateTime.UtcNow.Month+"_"+ DateTime.UtcNow.Year)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Excel/DailyPlan_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year));
                    }
                    string _path = Path.Combine(Server.MapPath("~/Content/Excel/DailyPlan_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year),"Plan_"+DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss")+"_"+ _FileName);
                    excel.SaveAs(_path);
                   
                    var Data=MiniExcel.Query<ImportDailyPlan>(_path);
                    if (Data.Count() < 1 || Data==null)
                    {
                        Response.StatusCode = 412;
                        return Json(new { Message = "No Data In Uploaded File" }, JsonRequestBehavior.AllowGet);
                    }
                    FileModel fm = new FileModel();
                    var fmdb = _ctx.FileModels.Where(x => x.UploadedAt == date).FirstOrDefault();
                    if (fmdb == null)
                    {
                        fm.FileTitle = "DailyPlan";
                        fm.FileLocation = _path;
                        fm.UploadedAt = date.Date;
                        _ctx.FileModels.Add(fm);
                    }
                    else
                    {
                        fm = fmdb;
                        fm.FileLocation = _path;
                    }
               
                    _ctx.SaveChanges();

                    List<DailyPlan> dp= new List<DailyPlan>();
                     
                    foreach (var a in Data)
                    {
                        var cfaid = _ctx.Cfa.Where(x => x.DepoCode == a.CFA).FirstOrDefault().CfaId;
                        var skuid = _ctx.Sku.Where(x => x.SkuCode == a.SKU).FirstOrDefault().SkuId;
                        var plan = _ctx.DailyPlan.Where(x => x.CfaId == cfaid && x.SkuId == skuid && x.PlanDate == date).FirstOrDefault();
                       
                        if (plan != null)
                        {
                            plan.PriorityQty = a.Priority;
                            plan.SIT = a.SIT;
                            plan.SHQ = a.Priority - a.SIT;
                            plan.PlanDate = date;
                            _ctx.SaveChanges();
                           
                        }
                        else
                        {
                           var ndp=  new DailyPlan
                            {
                                CfaId = cfaid,
                                SkuId = skuid,
                                PriorityQty = a.Priority,
                                SIT = a.SIT,
                                SHQ = a.Priority - a.SIT,
                                PlanDate = date
                            };
                            _ctx.DailyPlan.Add(ndp);
                            _ctx.SaveChanges();
                          
                        }
                       
                    }
                    Prioritization.Prioritize(date,_ctx);

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

      
    }
}