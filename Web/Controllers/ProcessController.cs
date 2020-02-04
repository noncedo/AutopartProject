using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class ProcessController : Controller
    {
        IRepository repository;

        public ProcessController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            var processes = repository.Get<Process>().ToList();
            return View(processes);
        }

        [HttpPost]
        public JsonResult Update(int processId, string name)
        {
            var process = repository.Get<Process>().Where(x => x.ProcessId == processId).FirstOrDefault();
            process.Name = name;

            repository.Update(process);
            repository.Save();
            return Json(new { success = true});
        }

        [HttpPost]
        public JsonResult Delete(int processId)
        {
            var processRuns = repository.Get<ProcessRun>().Where(x => x.ProcessId == processId).ToList();
            foreach(var runs in processRuns)
            {
                repository.Delete<ProcessRun>(runs);
            }
            repository.Save();
            repository.Delete<Process>(processId);
            repository.Save();

            return Json(new { success = true});
        }

    }
}