using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;


namespace Web.Controllers
{
    public class ProcessRunController : Controller
    {
        IRepository repository;

        public ProcessRunController(IRepository repository)
        {
            this.repository = repository;
        }

        //[HttpGet]
        //public ActionResult Index()
        //{
        //    ViewData["User"] = HttpContext.User.Identity.Name;
            
        //    return View();
        //}

        [HttpGet]
        public ActionResult Index(int? ProcessId)
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            var processRuns = repository.Get<vwProcessRunConsolidation>().Where(x => x.ProcessId == ProcessId.Value).ToList();
            return View(processRuns);
        }
    }
}