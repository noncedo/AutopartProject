using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class SettingController : Controller
    {
        IRepository repository;

        public SettingController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["Processes"] = repository.Get<Process>();
            return View();
        }
    }
}