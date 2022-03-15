using Loadability.Models;
using Loadability.ViewModels;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loadability.Controllers
{
    public class PrDetailsController : Controller
    {
        // GET: PrDetails
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(PrDetails pr)
        {
            var ctx = new ApplicationDbContext();
            var prd = ctx.PrDetails.Where(x => x.CfaId == pr.CfaId && x.SkuId == pr.SkuId && x.PrDate == pr.PrDate).FirstOrDefault();
            if (prd != null)
            {
                prd.PrQty = pr.PrQty;
                ctx.SaveChanges();
            }
            else
            {
                pr.PrDate = pr.PrDate.Date;
                ctx.PrDetails.Add(pr);
                ctx.SaveChanges();
                prd = pr;
            }
            return Json(prd, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPr(PrDetails pr)
        {
            var ctx = new ApplicationDbContext();
            pr.PrDate = pr.PrDate.Date;
            var prd = ctx.PrDetails.Where(x => x.CfaId == pr.CfaId && x.SkuId == pr.SkuId && x.PrDate == pr.PrDate).FirstOrDefault();
            if(prd!=null)
            return Json(prd , JsonRequestBehavior.AllowGet);
            else
            {
                Response.StatusCode = 404;
                return Json(new { Message= "Not Found"}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadExcel(DailyPlanDto updp)
        {
            var excel = updp.excel;
            var date = Convert.ToDateTime(updp.SaveDate);
            var _ctx = new ApplicationDbContext();

            try
            {
                if (excel.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(excel.FileName);
                    if (!Directory.Exists(Server.MapPath("~/Content/Excel/PrDetails_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Excel/PrDetails_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year));
                    }
                    string _path = Path.Combine(Server.MapPath("~/Content/Excel/PrDetails_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year), "Plan_" + DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss") + "_" + _FileName);
                    excel.SaveAs(_path);
                    var Data = MiniExcel.Query<PrUploadDto>(_path);
                    if (Data.Count() < 1 || Data==null)
                    {
                        Response.StatusCode = 412;
                        return Json(new { Message = "No Data In Uploaded File" }, JsonRequestBehavior.AllowGet);
                    }
                    FileModel fm = new FileModel();
                    var fmdb = _ctx.FileModels.Where(x => x.UploadedAt == date).FirstOrDefault();
                    if (fmdb == null)
                    {
                        fm.FileTitle = "PrDetails";
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
                    foreach(var j in Data)
                    {
                        var cfaid = _ctx.Cfa.Where(x => x.DepoCode == j.CFA).FirstOrDefault().CfaId;
                        var skuid = _ctx.Sku.Where(x => x.SkuCode == j.SKU).FirstOrDefault().SkuId;
                        var prDetails = _ctx.PrDetails.Where(x => x.CfaId == cfaid && x.SkuId == skuid && x.PrDate == date).FirstOrDefault();
                        if (prDetails != null)
                        {
                            prDetails.PrQty = j.Qty;
                            _ctx.SaveChanges();
                        }
                        else
                        {
                            prDetails = new PrDetails();
                            prDetails.PrQty = j.Qty;
                            prDetails.SkuId = skuid;
                            prDetails.CfaId = cfaid;
                            prDetails.PrDate = date;
                            _ctx.PrDetails.Add(prDetails);
                            _ctx.SaveChanges();
                        }
                    }
                    return Json(Data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new { Message = "Empty File Uploaded" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                // ViewBag.Message = "File upload failed!!";
                Response.StatusCode = 400;
                return Json("Unable To Save File, Make sure File is Selected");
            }


        }
    }
}