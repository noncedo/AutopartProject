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
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DataAccess;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Dealer>("Dealers");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DealersController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public DealersController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDealers()
        {
            try
            {
                var result = repository.GetQueryable<Dealer>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDealer([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<Dealer>(x => x.DealerGroupId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Dealer Dealer)
        {
            Validate(Dealer);

            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                repository.Update<Dealer>(Dealer);
                await repository.SaveAsync();
                return Updated(Dealer);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost]
        [EnableQuery]

        public async Task<IHttpActionResult> Post(Dealer Dealer)
        {
            try
            {
                var t = User.Identity.Name;
                var user = await repository.GetFirstAsync<User>(x => x.Username == t);
                repository.Create<Dealer>(Dealer);
                await repository.SaveAsync();
                return Created(Dealer);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpPatch]
        [AcceptVerbs("MERGE")]
        [EnableQuery]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Dealer> delta)
        {
            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                var entity = await repository.GetByIdAsync<Dealer>(key);

                delta.Patch(entity);
                repository.Update<Dealer>(entity);
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
                repository.Delete<Dealer>(key);
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
