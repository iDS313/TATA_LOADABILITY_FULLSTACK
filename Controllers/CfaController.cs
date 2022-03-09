using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loadability.Controllers
{
    public class CfaController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public CfaController()
        {
            _ctx = new ApplicationDbContext();
        }
        // GET: Cfa
        public ActionResult Index()
        {
            var cfa = _ctx.Cfa.ToList();
            return Json(new { cfa=cfa},JsonRequestBehavior.AllowGet);
        }
    }
}