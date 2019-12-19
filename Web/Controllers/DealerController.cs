using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Web.Controllers
{
    public class DealerController : Controller
    {
        IRepository repository;

        public DealerController(IRepository repository)
        {
            this.repository = repository;
        }


        [HttpGet]
        public ActionResult Index()
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            ViewData["DealerGroups"] = repository.Get<DealerGroup>();
            return View(repository.Get<Dealer>().ToList());
        }

        [HttpPost]
        public JsonResult UpdateDealer(int id, int groupId, string name, string code, string dealer)
        {
            var dealerObj = new Dealer()
            {
                DealerId = id,
                DealerGroupId = groupId,
                Code = code,
                Name = name
            };

            repository.Update(dealerObj);
            repository.Save();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Save(int groupId, string name, string code)
        {
            var newDealer = new Dealer()
            {
                Code = code,
                Name = name,
                DealerGroupId = groupId,
                IsActive = true
            };

            repository.Create<Dealer>(newDealer);
            repository.Save();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Delete(int dealerId)
        {
            repository.Delete<Dealer>(dealerId);
            repository.Save();

            return Json(new { success = true });
        }
    }
}