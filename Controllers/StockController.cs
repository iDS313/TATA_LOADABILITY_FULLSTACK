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
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        // GET: Stock

        public StockController()
        {
            _ctx = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Save(StockDetails sd)
        {
            var s = _ctx.StockDetails.Where(x => x.SkuId == sd.SkuId ).FirstOrDefault();
            if (s == null)
            {
                sd.RecordedAt = sd.RecordedAt.Date;
                sd.AvailableQty = sd.StartQty;
                _ctx.StockDetails.Add(sd);
                _ctx.SaveChanges();
                return Json(new { SkuId = sd.SkuId.ToString(), RecordedAt = sd.RecordedAt, StartQty = sd.StartQty },JsonRequestBehavior.AllowGet);
            }
            else
            {
                s.StartQty = sd.StartQty+ s.AvailableQty;
                s.AvailableQty = sd.StartQty;
                _ctx.SaveChanges();
                return Json(new { SkuId = s.SkuId.ToString(), RecordedAt = s.RecordedAt, StartQty = s.StartQty },JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult getStock(StockDetails sd)
        {
            var date = sd.RecordedAt.Date;
            var st = _ctx.StockDetails.Where(x => x.SkuId == sd.SkuId).FirstOrDefault();
            if (st == null)
            {
                st = sd;
                st.StartQty = 0;
            }
            return Json(st, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadExcel(DailyPlanDto updp)
        {
            var excel = updp.excel;
            var date = Convert.ToDateTime(updp.SaveDate);
            try
            {
                if (excel.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(excel.FileName);
                    if (!Directory.Exists(Server.MapPath("~/Content/Excel/StockDetail_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Excel/StockDetail_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year));
                    }
                    string _path = Path.Combine(Server.MapPath("~/Content/Excel/StockDetail_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year), "Plan_" + DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss") + "_" + _FileName);
                    excel.SaveAs(_path);
                    var Data = MiniExcel.Query<StockDto>(_path);
                    if (Data.Count() < 1 || Data == null)
                    {
                        Response.StatusCode = 412;
                        return Json(new { Message = "No Data In Uploaded File" }, JsonRequestBehavior.AllowGet);
                    }
                    FileModel fm = new FileModel();
                    var fmdb = _ctx.FileModels.Where(x => x.UploadedAt == date).FirstOrDefault();
                    if (fmdb == null)
                    {
                        fm.FileTitle = "StockDetails";
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
                    foreach(var a in Data)
                    {
                        var skuid = _ctx.Sku.Where(x => x.SkuCode == a.SKU).FirstOrDefault().SkuId;
                        var d = date.Date;
                        var s = _ctx.StockDetails.Where(x => x.SkuId == skuid).FirstOrDefault();
                        if (s != null)
                        {
                            s.StartQty = s.AvailableQty + a.Quantity;
                            s.AvailableQty = s.StartQty;
                            s.RecordedAt = d;
                            _ctx.SaveChanges();
                        }
                        else
                        {
                            var sd = new StockDetails();
                            sd.SkuId = skuid;
                            sd.RecordedAt = d;
                            sd.StartQty = a.Quantity;
                            sd.AvailableQty = a.Quantity;
                            _ctx.StockDetails.Add(sd);
                            _ctx.SaveChanges();
                        }
                    }
                    return Json(new { Message = "Data Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = 403;
                return Json(new { Message = "Empty File Uploaded, Or File contains Invalid data" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { Message=e.Message},JsonRequestBehavior.AllowGet);
            }
        }

    }
}