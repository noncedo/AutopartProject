using DataAccess;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;

namespace Web.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DataAccess;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DmsProvider>("DmsProviders");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DmsProvidersController : ODataController
    {
        IRepository repository;
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        public DmsProvidersController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDmsProviders()
        {
            try
            {
                var result = repository.GetQueryable<DmsProvider>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // GET: odata/DealerGroups(5)
        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDmsProviders([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<DmsProvider>(x => x.DmsProviderId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // PUT: odata/DealerGroups(5)
        [HttpPut]
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, DmsProvider DmsProvider)
        {
            Validate(DmsProvider);

            try
            {
                repository.Update<DmsProvider>(DmsProvider);
                await repository.SaveAsync();
                return Updated(DmsProvider);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost]
        [EnableQuery]
        // POST: odata/DealerGroups
        public async Task<IHttpActionResult> Post(DmsProvider DmsProvider)
        {
            try
            {
                repository.Create<DmsProvider>(DmsProvider);
                await repository.SaveAsync();
                return Created(DmsProvider);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // PATCH: odata/DealerGroups(5)
        [HttpPatch]
        [AcceptVerbs("MERGE")]
        [EnableQuery]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<DmsProvider> delta)
        {
            try
            {
                var entity = await repository.GetByIdAsync<DmsProvider>(key);
                delta.Patch(entity);
                repository.Update<DmsProvider>(entity);
                await repository.SaveAsync();
                return Updated(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            // TODO: Get the entity here.

            // delta.Patch(DmsProvider);

            // TODO: Save the patched entity.

            // return Updated(DmsProvider);
        }

        [HttpDelete]
        [EnableQuery]
        // DELETE: odata/DealerGroups(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                repository.Delete<DmsProvider>(key);
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
