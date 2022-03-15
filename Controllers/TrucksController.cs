using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loadability.Controllers
{
    public class TrucksController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        // GET: Trucks
        public TrucksController()
        {
            _ctx = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var trucks = _ctx.Trucks.Select(x=>new { 
            TruckId=x.TruckId.ToString(),
            x.Name,
            x.Limit,
            x.Capacity
            }).ToList();
            return Json(trucks, JsonRequestBehavior.AllowGet);
        }
    }
}