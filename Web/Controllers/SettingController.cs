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
            ViewData["User"] = HttpContext.User.Identity.Name;

            ViewData["Processes"] = repository.Get<Process>();

            return View(repository.Get<Setting>().ToList());
        }

        [HttpPost]
        public JsonResult UpdateSetting(int id, string value, int processId,string key)
        {
            var settingObj = new Setting()
            {
              SettingId = id,
              Key=key,
              ProcessId = processId,
              Value = value

            };

            repository.Update(settingObj);
            repository.Save();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            repository.Delete<Setting>(id);
            repository.Save();

            return Json(new { success = true });
        }
    }
}