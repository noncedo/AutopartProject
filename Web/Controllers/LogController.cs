using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class LogController : Controller
    {
        IRepository repository;

        public LogController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index(int? ProcessRunId)
        {
            ViewData["LogTypes"] = repository.Get<LogType>();
            var logs = repository.Get<Log>().Where(x => x.ProcessRunID == ProcessRunId).ToList();
            return View(logs);
        }
    }
}