using DataAccess;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;

namespace Web.Controllers
{
    //To Do: Add valdtion to prevent things like duplicate data or incorrect input. 
    public class UsersController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public UsersController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetUsers()
        {
            try
            {
                var result = repository.GetQueryable<User>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetUser([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<User>(x => x.UserId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        // PUT: odata/Users(5)
        [HttpPut]
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, User User)
        {
            Validate(User);

            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                repository.Update<User>(User);
                await repository.SaveAsync();
                return Updated(User);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpPost]
        [EnableQuery]
        public async Task<IHttpActionResult> Post(User userNew)
        {
            try
            {
                //Adds the logged in user in the createdBy field. To see who added the new user to the system
                //userNew = the new created user.
                var t = User.Identity.Name;
                var user = await repository.GetFirstAsync<User>(x => x.Username == t);
                repository.Create<User>(userNew);
                await repository.SaveAsync();
                return Created(userNew);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpPatch]
        [AcceptVerbs("MERGE")]
        [EnableQuery]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<User> delta)
        {
            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                var entity = await repository.GetByIdAsync<User>(key);

                delta.Patch(entity);
                repository.Update<User>(entity);
                await repository.SaveAsync();
                return Updated(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                repository.Delete<User>(key);
                repository.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
