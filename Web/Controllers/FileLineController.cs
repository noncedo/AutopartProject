using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class FileLineController : Controller
    {
        IRepository repository;

        public FileLineController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index(int? FileSourceId, int? FileDestinationId)
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            var viewModel = new FileLineIndexViewModel { FileSourceId = FileSourceId, FileDestinationId = FileDestinationId };
            return View(viewModel);
        }
    }
}