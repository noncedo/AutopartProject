using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class SourceFileController : Controller
    {
        IRepository repository;

        public SourceFileController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index(int? ProcessRunId)
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            var sourceFiles = repository.Get<FileSource>().Where(x => x.ProcessRunId == ProcessRunId.Value).ToList();
            return View(sourceFiles);
        }
    }
}