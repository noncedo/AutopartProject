using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;

namespace Web.Controllers
{
    public class UserController : Controller
    {
       
            IRepository repository;

            public UserController(IRepository repository)
            {
                this.repository = repository;
            }

            [HttpGet]
            public ActionResult Index()
            {
                ViewData["User"] = HttpContext.User.Identity.Name;
                var users = repository.Get<User>().ToList();
                return View(users);
            }

        [HttpPost]
        public JsonResult UpdateUser(int userId, string username, string domain)
        {
            var userObj = new User()
            {
                UserId = userId,
                Username = username,
                Domain = domain,
                
            };
            repository.Update(userObj);
            repository.Save();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Delete(int userId)
        {
            repository.Delete<User>(userId);
            repository.Save();

            return Json(new { success = true });
        }

    }
}