using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class DmsProviderController : Controller
    {
        IRepository repository;

        public DmsProviderController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: DmsProvider
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["User"] = HttpContext.User.Identity.Name;

            return View(repository.Get<DmsProvider>().ToList());
        }
        [HttpPost]
        public JsonResult UpdateDMS(int dmsid, string name)
        {
            var dmsObj = new DmsProvider()
            {
              DmsProviderId = dmsid,
              Name = name
            };

            repository.Update(dmsObj);
            repository.Save();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            repository.Delete<DmsProvider>(id);
            repository.Save();

            return Json(new { success = true });
        }
    }
}