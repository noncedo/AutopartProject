using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class DestinationFileController : Controller
    {
        IRepository repository;

        public DestinationFileController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index(int? ProcessRunId)
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            
            return View(ProcessRunId);
        }
    }
}