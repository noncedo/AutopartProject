﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class DealerGroupController : Controller
    {

        IRepository repository;

        public DealerGroupController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["User"] = HttpContext.User.Identity.Name;
            ViewData["DmsProviders"] = repository.Get<DmsProvider>();

            return View(repository.Get<DealerGroup>().ToList());
        }

        [HttpPost]
        public JsonResult UpdateDealerGroup(int id,string code,int dealerGroupId,string name, int dmsProviderId)
        {
            var dealerGroupObj = new DealerGroup()
            {
               DealerGroupId = id,
               Code=code,
              DmsProviderId = dmsProviderId,
               Name = name

            };

            repository.Update(dealerGroupObj);
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