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
          

        }

        [HttpPost]
        public ActionResult getWeight(PlanReq pl)
        {


        }
    
    }
}

