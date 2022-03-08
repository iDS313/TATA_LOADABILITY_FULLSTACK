using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loadability.Controllers
{
    public class SkuController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public SkuController()
        {
            _ctx = new ApplicationDbContext();
        }
        // GET: Sku
        public ActionResult Index()
        {
            var sku = _ctx.Sku;
            return Json(new {sku=sku },JsonRequestBehavior.AllowGet);
        }
    }
}